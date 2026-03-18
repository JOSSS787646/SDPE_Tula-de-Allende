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

            bool isExpired = request.CompleteMaximeDate.HasValue &&
                             DateTime.Now > request.CompleteMaximeDate.Value;

            if (!request.IdAcquisitionClassification.HasValue)
                return;

            // ============================================================
            // DOCUMENTOS
            // ============================================================
            var allDocs = await _classificationRepository
                .GetRequiredByClassification(request.IdAcquisitionClassification.Value);

            var exceptions = await _exceptionRepository
                .GetActiveByRequestId(requestId);

            var uploadedDocs = await _documentRepository
                .GetActiveByRequestId(requestId);

            var obligatorios = allDocs
                .Where(r =>
                    r.IsRequired == true &&
                    !exceptions.Any(e =>
                        e.IdDocumentType == r.DocumentTypeId &&
                        e.DoesNotApply == true &&
                        e.Active == true))
                .ToList();

            bool allApproved = true;

            // ============================================================
            // VALIDAR DOCUMENTOS
            // ============================================================
            foreach (var req in obligatorios)
            {
                var docsOfType = uploadedDocs
                    .Where(d => d.DocumentTypeId == req.DocumentTypeId)
                    .ToList();

                if (!docsOfType.Any())
                {
                    allApproved = false;
                    continue;
                }

                foreach (var doc in docsOfType)
                {
                    if (doc.IdDocumentStatus != (int)DocumentStatusEnum.Aprobado)
                    {
                        allApproved = false;
                    }
                }
            }

            // ============================================================
            // 🔥 NUEVA LÓGICA FINAL
            // ============================================================

            Console.WriteLine("============================================");
            Console.WriteLine($"RequestId: {requestId}");
            Console.WriteLine($"isExpired: {isExpired}");
            Console.WriteLine($"allApproved: {allApproved}");

            if (allApproved)
            {
                // ✅ COMPLETO SIEMPRE GANA
                request.IdApplicationStatus = (int)RequestStatusEnum.Completo;
                Console.WriteLine("🟢 COMPLETO (aunque esté vencido)");
            }
            else if (isExpired)
            {
                // ❌ vencido y no completo
                request.IdApplicationStatus = (int)RequestStatusEnum.Incompleto;
                Console.WriteLine("🔴 INCOMPLETO (vencido)");
            }
            else
            {
                // ⏳ en proceso
                request.IdApplicationStatus = (int)RequestStatusEnum.EnRevision;
                Console.WriteLine("🟡 EN REVISIÓN");
            }

            Console.WriteLine("============================================");

            await _requestRepository.UpdateAsync(request);
        }
    }
}