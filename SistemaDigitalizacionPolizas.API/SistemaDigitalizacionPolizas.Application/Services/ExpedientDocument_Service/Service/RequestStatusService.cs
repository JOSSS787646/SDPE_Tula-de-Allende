using SistemaDigitalizacionPolizas.Domain.Enums;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences;
using SSistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service
{
    public class RequestStatusService : IRequestStatusService
    {
        private readonly IAcquisitionRequest _requestRepository;
        private readonly IDocumentExpedientRepository _documentRepository;
        private readonly IClasificationDocumentTypeRepository _classificationRepository;
        private readonly IRequestDocumentExceptionRepository _exceptionRepository;
        private readonly IApplicationStatusRepository _statusRepository;
        private readonly IUnitOfWorkService _unitOfWork;

        public RequestStatusService(
            IAcquisitionRequest requestRepository,
            IDocumentExpedientRepository documentRepository,
            IClasificationDocumentTypeRepository classificationRepository,
            IRequestDocumentExceptionRepository exceptionRepository,
            IApplicationStatusRepository statusRepository,
            IUnitOfWorkService unitOfWork)
        {
            _requestRepository = requestRepository;
            _documentRepository = documentRepository;
            _classificationRepository = classificationRepository;
            _exceptionRepository = exceptionRepository;
            _statusRepository = statusRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task RecalculateStatus(int requestId)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);

            if (request == null)
                throw new Exception("Solicitud no encontrada.");

            if (!request.IdAcquisitionClassification.HasValue)
                return;

            // 1️⃣ TODOS los documentos de la clasificación
            var allDocs = await _classificationRepository
                .GetRequiredByClassification(request.IdAcquisitionClassification.Value);

            // 2️⃣ Excepciones activas de esta solicitud
            var exceptions = await _exceptionRepository
                .GetActiveByRequestId(requestId);

            // 3️⃣ Documentos cargados activos (SIN filtrar por idEstadoDocumento)
            var uploadedDocs = await _documentRepository
                .GetActiveByRequestId(requestId);

            // 4️⃣ Separar obligatorios y no obligatorios
            var obligatorios = allDocs
                .Where(r =>
                    r.IsRequired == true &&
                    !exceptions.Any(e =>
                        e.IdDocumentType == r.DocumentTypeId &&
                        e.DoesNotApply == true &&
                        e.Active == true))
                .ToList();

            var noObligatorios = allDocs
                .Where(r =>
                    r.IsRequired == false ||
                    exceptions.Any(e =>
                        e.IdDocumentType == r.DocumentTypeId &&
                        e.DoesNotApply == true &&
                        e.Active == true))
                .ToList();

            // ---- LOGS GENERALES ----
            Console.WriteLine("============================================");
            Console.WriteLine($"[RecalculateStatus] RequestId: {requestId}");
            Console.WriteLine($"  Docs TOTAL en clasificación: {allDocs.Count}");
            Console.WriteLine($"  Obligatorios a evaluar:      {obligatorios.Count}");
            Console.WriteLine($"  No obligatorios / excepción: {noObligatorios.Count} → AUTO-APROBADO");
            Console.WriteLine($"  Excepciones activas:         {exceptions.Count}");
            Console.WriteLine($"  UploadedDocs TOTAL:          {uploadedDocs.Count}");
            Console.WriteLine("--------------------------------------------");

            foreach (var noReq in noObligatorios)
            {
                Console.WriteLine($"  DocTypeId: {noReq.DocumentTypeId}");
                Console.WriteLine($"    No obligatorio / con excepción → ✅ AUTO-APROBADO");
            }

            bool allApproved = true;

            // 5️⃣ Evaluar obligatorios — TODOS los docs del tipo deben estar Aprobados
            foreach (var req in obligatorios)
            {
                var docsOfType = uploadedDocs
                    .Where(d => d.DocumentTypeId == req.DocumentTypeId)
                    .ToList();

                Console.WriteLine($"  DocTypeId: {req.DocumentTypeId}");
                Console.WriteLine($"    Docs encontrados: {docsOfType.Count}");

                // Sin documentos → incompleto
                if (!docsOfType.Any())
                {
                    allApproved = false;
                    Console.WriteLine($"    ⚠️  SIN DOCUMENTO → incompleto");
                    continue;
                }

                // ✅ CAMBIO CLAVE: evaluar CADA documento individualmente
                bool typeIsApproved = true;

                foreach (var doc in docsOfType)
                {
                    var statusName = doc.IdDocumentStatus switch
                    {
                        (int)DocumentStatusEnum.Aprobado => "Aprobado ✅",
                        (int)DocumentStatusEnum.Cargado => "Cargado 📄",
                        (int)DocumentStatusEnum.Observado => "Observado 👁️",
                        _ => $"Desconocido ({doc.IdDocumentStatus})"
                    };

                    Console.WriteLine($"      DocId: {doc.Id} | Estado: {statusName}");

                    // Si CUALQUIER documento del tipo NO está aprobado → el tipo falla
                    if (doc.IdDocumentStatus != (int)DocumentStatusEnum.Aprobado)
                    {
                        typeIsApproved = false;
                        Console.WriteLine($"      ❌ No aprobado → este tipo queda incompleto");
                    }
                }

                if (typeIsApproved)
                {
                    Console.WriteLine($"    ✅ Todos aprobados → OK");
                }
                else
                {
                    allApproved = false;
                    Console.WriteLine($"    ❌ Al menos uno sin aprobar → solicitud incompleta");
                }
            }

            Console.WriteLine("--------------------------------------------");
            Console.WriteLine($"  allApproved: {allApproved}");

            // 6️⃣ Asignar estado de SOLICITUD
            // EstadoSolicitud: 1 = Incompleto, 2 = Completo
            if (allApproved)
            {
                request.IdApplicationStatus = 2;
                Console.WriteLine("  🟢 ESTADO SOLICITUD → COMPLETO (id: 2)");
            }
            else
            {
                request.IdApplicationStatus = 1;
                Console.WriteLine("  🔴 ESTADO SOLICITUD → INCOMPLETO (id: 1)");
            }

            Console.WriteLine("============================================");

            await _requestRepository.UpdateAsync(request);
        }
    }
}