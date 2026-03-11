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

            // 1️⃣ Documentos requeridos por clasificación (solo obligatorios activos)
            var requiredDocs = await _classificationRepository
                .GetRequiredByClassification(request.IdAcquisitionClassification.Value);

            // 2️⃣ Excepciones activas de esta solicitud
            var exceptions = await _exceptionRepository
                .GetActiveByRequestId(requestId);

            // 3️⃣ Documentos cargados activos (SIN filtrar por idEstadoDocumento)
            var uploadedDocs = await _documentRepository
                .GetActiveByRequestId(requestId);

            // 4️⃣ Documentos obligatorios quitando los que tienen excepción válida
            var requiredWithoutExceptions = requiredDocs
                .Where(r =>
                    r.IsRequired == true &&
                    !exceptions.Any(e =>
                        e.IdDocumentType == r.DocumentTypeId &&
                        e.DoesNotApply == true &&
                        e.Active == true))
                .ToList();

            // ---- LOGS GENERALES ----
            Console.WriteLine("============================================");
            Console.WriteLine($"[RecalculateStatus] RequestId: {requestId}");
            Console.WriteLine($"  RequiredDocs TOTAL:          {requiredDocs.Count}");
            Console.WriteLine($"  Obligatorios:                {requiredDocs.Count(r => r.IsRequired == true)}");
            Console.WriteLine($"  Excepciones activas:         {exceptions.Count}");
  
            Console.WriteLine($"  UploadedDocs TOTAL:          {uploadedDocs.Count}");
            Console.WriteLine("--------------------------------------------");

            bool allApproved = true;

            foreach (var req in requiredWithoutExceptions)
            {
                // Documentos activos de este tipo para esta solicitud
                var docsOfType = uploadedDocs
                    .Where(d => d.DocumentTypeId == req.DocumentTypeId)
                    .ToList();

                // Estado de cada documento encontrado
                bool hasApproved = docsOfType.Any(d =>
                    d.IdDocumentStatus == (int)DocumentStatusEnum.Aprobado);

                bool hasCargado = docsOfType.Any(d =>
                    d.IdDocumentStatus == (int)DocumentStatusEnum.Cargado);

                bool hasObservado = docsOfType.Any(d =>
                    d.IdDocumentStatus == (int)DocumentStatusEnum.Observado);

                Console.WriteLine($"  DocTypeId: {req.DocumentTypeId}");
                Console.WriteLine($"    Docs encontrados : {docsOfType.Count}");
                Console.WriteLine($"    Aprobado         : {hasApproved}");
                Console.WriteLine($"    Cargado          : {hasCargado}");
                Console.WriteLine($"    Observado        : {hasObservado}");

                if (!docsOfType.Any())
                {
                    allApproved = false;
                    Console.WriteLine($"    ⚠️  SIN DOCUMENTO → solicitud incompleta");
                    continue;
                }

                // Solo cuenta como OK si tiene al menos uno Aprobado (idEstadoDocumento = 2)
                if (hasApproved)
                {
                    Console.WriteLine($"    ✅ Aprobado → OK");
                    continue;
                }

                // Tiene docs pero ninguno aprobado (puede ser Cargado u Observado)
                allApproved = false;

                if (hasObservado)
                    Console.WriteLine($"    👁️  Observado sin aprobado → solicitud incompleta");
                else
                    Console.WriteLine($"    📄 Solo Cargado, sin aprobado → solicitud incompleta");
            }

            Console.WriteLine("--------------------------------------------");
            Console.WriteLine($"  allApproved: {allApproved}");

            // 5️⃣ Asignar estado de SOLICITUD directo por ID
            // EstadoSolicitud: 1 = Incompleto, 2 = Completo
            if (allApproved)
            {
                request.IdApplicationStatus = 2; // Completo
                Console.WriteLine("  🟢 ESTADO SOLICITUD → COMPLETO (id: 2)");
            }
            else
            {
                request.IdApplicationStatus = 1; // Incompleto
                Console.WriteLine("  🔴 ESTADO SOLICITUD → INCOMPLETO (id: 1)");
            }

            Console.WriteLine("============================================");

            await _requestRepository.UpdateAsync(request);
        }
    }
}