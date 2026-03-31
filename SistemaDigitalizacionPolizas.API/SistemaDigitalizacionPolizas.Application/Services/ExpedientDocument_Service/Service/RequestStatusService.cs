using SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument;
using SistemaDigitalizacionPolizas.Domain.Enums;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Domain.Services;
using SistemaDigitalizacionPolizas.Domain.Specifications;
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
        private readonly RequestStatusEvaluator _statusEvaluator;

        public RequestStatusService(
        IAcquisitionRequest requestRepository,
        IDocumentExpedientRepository documentRepository,
        IClasificationDocumentTypeRepository classificationRepository,
        IRequestDocumentExceptionRepository exceptionRepository,
        IApplicationStatusRepository statusRepository,
        IUnitOfWorkService unitOfWork,
        RequestStatusEvaluator statusEvaluator) // ✅ CORRECTO
        {
            _requestRepository = requestRepository;
            _documentRepository = documentRepository;
            _classificationRepository = classificationRepository;
            _exceptionRepository = exceptionRepository;
            _statusRepository = statusRepository;
            _unitOfWork = unitOfWork;
            _statusEvaluator = statusEvaluator;
        }

        // ============================================================
        // 🔹 TU MÉTODO ORIGINAL (NO TOCADO)
        // ============================================================
        // ── MÉTODO REFACTORIZADO ─────────────────────────────────────────────
        public async Task RecalculateStatus(int requestId)
        {
            var request = await _requestRepository.GetStatusDataAsync(requestId);
            if (request is null)
                throw new Exception("Solicitud no encontrada");

            IEnumerable<dynamic> documents;
            try { documents = await _documentRepository.GetActiveByRequestId(requestId); }
            catch (Exception ex) { throw new Exception("FALLA EN: GetActiveByRequestId documentos", ex); }

            IEnumerable<dynamic> requiredDocs;
            try
            {
                requiredDocs = await _classificationRepository
                    .GetRequiredByClassification(request.IdAcquisitionClassification.Value);
            }
            catch (Exception ex) { throw new Exception("FALLA EN: GetRequiredByClassification", ex); }

            IEnumerable<dynamic> exceptions;
            try { exceptions = await _exceptionRepository.GetActiveByRequestId(requestId); }
            catch (Exception ex) { throw new Exception("FALLA EN: GetActiveByRequestId exceptions", ex); }

            var obligatorios = requiredDocs
                .Where(r =>
                    r.IsRequired &&
                    !exceptions.Any(e =>
                        e.IdDocumentType == r.DocumentTypeId &&
                        e.DoesNotApply &&
                        e.Active))
                .ToList();

            bool hasObservado = false;
            bool allApproved = true;
            bool allLoaded = true;
            bool hasLoaded = false;
            int missingCount = 0;

            foreach (var req in obligatorios)
            {
                var docsOfType = documents
                    .Where(d => d.DocumentTypeId == req.DocumentTypeId)
                    .ToList();

                if (!docsOfType.Any())
                {
                    missingCount++;
                    allApproved = false;
                    allLoaded = false;
                    continue;
                }

                foreach (var doc in docsOfType)
                {
                    switch ((DocumentStatusEnum)doc.IdDocumentStatus)
                    {
                        case DocumentStatusEnum.Observado:
                            hasObservado = true;
                            allApproved = false;
                            allLoaded = false;
                            break;
                        case DocumentStatusEnum.Aprobado:
                            // cuenta como cargado también
                            break;
                        case DocumentStatusEnum.Cargado:
                            hasLoaded = true;
                            allApproved = false;
                            // ✅ NO tocar allLoaded aquí
                            break;
                        default:
                            allApproved = false;
                            allLoaded = false;
                            break;
                    }
                }

                // ✅ allLoaded = true solo si TODOS son Cargado o Aprobado
                bool typeFullyLoaded = docsOfType.All(d =>
                    d.IdDocumentStatus == (int)DocumentStatusEnum.Cargado ||
                    d.IdDocumentStatus == (int)DocumentStatusEnum.Aprobado);

                if (!typeFullyLoaded)
                    allLoaded = false;
            }

            bool isExpired = request.CompleteMaximeDate.HasValue &&
                             DateTime.Now > request.CompleteMaximeDate.Value;

            var ctx = new StatusEvaluationContext(
                HasObservado: hasObservado,
                AllApproved: allApproved,
                AllLoaded: allLoaded,
                HasLoaded: hasLoaded,
                HasMissing: missingCount > 0,
                IsExpired: isExpired
            );

            var newStatus = _statusEvaluator.Evaluate(ctx);

            try
            {
                if (request.IdApplicationStatus != (int)newStatus)
                    await _requestRepository.UpdateStatusAsync(requestId, (int)newStatus);
            }
            catch (Exception ex) { throw new Exception("FALLA EN: UpdateStatusAsync", ex); }
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