using SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument;
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

        // ============================================================
        // 🔹 TU MÉTODO ORIGINAL (NO TOCADO)
        // ============================================================
        public async Task RecalculateStatus(int requestId)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);

            if (request == null)
                throw new Exception("Solicitud no encontrada.");

            bool isExpired = request.CompleteMaximeDate.HasValue &&
                             DateTime.Now > request.CompleteMaximeDate.Value;

            if (!request.IdAcquisitionClassification.HasValue)
                return;

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

                bool allOfTypeApproved = docsOfType
                    .All(d => d.IdDocumentStatus == (int)DocumentStatusEnum.Aprobado);

                if (!allOfTypeApproved)
                {
                    allApproved = false;
                }
            }

            if (allApproved)
            {
                request.IdApplicationStatus = (int)RequestStatusEnum.Completo;
            }
            else if (isExpired)
            {
                request.IdApplicationStatus = (int)RequestStatusEnum.Incompleto;
            }
            else
            {
                request.IdApplicationStatus = (int)RequestStatusEnum.EnRevision;
            }

            await _requestRepository.UpdateAsync(request);
        }

        // ============================================================
        // 🔥 NUEVO MÉTODO (ESTADO GLOBAL POR TIPO)
        // ============================================================
        public async Task<List<DocumentGroupStatusDto>> GetDocumentGroupStatus(int requestId)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);

            if (request == null)
                throw new Exception("Solicitud no encontrada.");

            var allDocs = await _classificationRepository
                .GetRequiredByClassification(request.IdAcquisitionClassification.Value);

            var exceptions = await _exceptionRepository
                .GetActiveByRequestId(requestId);

            var uploadedDocs = await _documentRepository
                .GetActiveByRequestId(requestId);

            var obligatorios = allDocs
                .Where(r =>
                    r.IsRequired &&
                    !exceptions.Any(e =>
                        e.IdDocumentType == r.DocumentTypeId &&
                        e.DoesNotApply &&
                        e.Active))
                .ToList();

            var grouped = obligatorios.GroupBy(x => x.DocumentTypeId);

            var result = new List<DocumentGroupStatusDto>();

            foreach (var group in grouped)
            {
                var docsOfType = uploadedDocs
                    .Where(d => d.DocumentTypeId == group.Key)
                    .ToList();

                int uploaded = docsOfType.Count;
                int approved = docsOfType.Count(d => d.IdDocumentStatus == (int)DocumentStatusEnum.Aprobado);
                int observed = docsOfType.Count(d => d.IdDocumentStatus == (int)DocumentStatusEnum.Observado);
                int loaded = docsOfType.Count(d => d.IdDocumentStatus == (int)DocumentStatusEnum.Cargado);
                int required = group.Count();

                DocumentGroupStatus status;

                // 1. Pendiente — no hay ningún documento subido
                if (uploaded == 0)
                {
                    status = DocumentGroupStatus.Pendiente;
                }
                // 2. Observado — al menos uno está observado (máxima prioridad negativa)
                else if (observed > 0)
                {
                    status = DocumentGroupStatus.Observado;
                }
                // 3. Cargado — al menos uno está en estado Cargado
                //    (no importa que los demás estén aprobados)
                else if (loaded > 0)
                {
                    status = DocumentGroupStatus.Cargado;
                }
                // 4. Completo — todos están aprobados y se cubre la cantidad requerida
                else if (approved >= required &&
                         docsOfType.All(d => d.IdDocumentStatus == (int)DocumentStatusEnum.Aprobado))
                {
                    status = DocumentGroupStatus.Completo;
                }
                // 5. Fallback — hay documentos pero ninguno encaja en los casos anteriores
                else
                {
                    status = DocumentGroupStatus.Cargado;
                }

                result.Add(new DocumentGroupStatusDto
                {
                    DocumentTypeId = group.Key,
                    DocumentName = group.First().DocumentType.DocumentName,
                    TotalRequired = required,
                    TotalUploaded = uploaded,
                    TotalApproved = approved,
                    Status = status
                });
            }

            return result;
        }
    }
}