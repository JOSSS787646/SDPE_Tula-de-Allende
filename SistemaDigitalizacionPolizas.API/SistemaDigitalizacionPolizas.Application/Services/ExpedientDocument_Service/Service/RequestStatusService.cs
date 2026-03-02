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

            var requiredDocs = await _classificationRepository
                .GetRequiredByClassification(request.IdAcquisitionClassification.Value);

            var exceptions = await _exceptionRepository
                .GetActiveByRequestId(requestId);

            var uploadedDocs = await _documentRepository
                .GetActiveByRequestId(requestId);

            var requiredWithoutExceptions = requiredDocs
                .Where(r => !exceptions.Any(e =>
                    e.IdDocumentType == r.DocumentTypeId &&
                    e.DoesNotApply &&
                    e.Active == true))
                .ToList();

            bool allCompleted = requiredWithoutExceptions.All(req =>
                uploadedDocs.Any(u =>
                    u.DocumentTypeId == req.DocumentTypeId &&
                    u.IdDocumentStatus == 1 &&
                    u.Active == true));

            var incompleteStatus = await _statusRepository.GetByCodeAsync(1);
            var completeStatus = await _statusRepository.GetByCodeAsync(2);

            request.IdApplicationStatus = allCompleted
                ? completeStatus.IdApplicationStatus
                : incompleteStatus.IdApplicationStatus;

            await _requestRepository.UpdateAsync(request);

            // ❌ NO COMMIT AQUÍ
        }
    }
}
