using Microsoft.Extensions.Logging;
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



/// <summary>
/// Servicio de aplicación encargado de gestionar y recalcular el estado de una solicitud
/// en función de sus documentos asociados, reglas de negocio y condiciones actuales.
///
/// Responsabilidad principal:
/// - Evaluar el estado global de una solicitud (Completo, Incompleto, En revisión, Observado).
/// - Analizar documentos requeridos, cargados, aprobados, observados y excepciones.
/// - Determinar el estado mediante reglas encapsuladas en el patrón Specification.
/// - Actualizar el estado en la base de datos si hay cambios.
/// - Obtener el estado agrupado por tipo de documento.
///
/// Dependencias:
/// - Repositorios para obtener datos de solicitudes, documentos, clasificaciones y excepciones.
/// - UnitOfWork para control transaccional.
/// - RequestStatusEvaluator para aplicar reglas de negocio de estados.
///
///
/// ------------------------------------------------------------
///
/// Importante:
/// - Aplica reglas de negocio complejas sin acoplarlas a controladores.
/// - Usa patrón Specification para mantener lógica desacoplada y extensible.
/// - Maneja excepciones específicas para facilitar debugging.
/// - No modifica datos innecesariamente (solo actualiza si hay cambio).
///
/// En resumen:
/// Este servicio centraliza la lógica de evaluación de estados de solicitudes
/// y documentos, asegurando consistencia, escalabilidad y mantenibilidad.
/// </summary>
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
        private readonly ILogger<RequestStatusService> _logger; // ✅ AGREGADO

        public RequestStatusService(
        IAcquisitionRequest requestRepository,
        IDocumentExpedientRepository documentRepository,
        IClasificationDocumentTypeRepository classificationRepository,
        IRequestDocumentExceptionRepository exceptionRepository,
        IApplicationStatusRepository statusRepository,
        IUnitOfWorkService unitOfWork,
        RequestStatusEvaluator statusEvaluator,
        ILogger<RequestStatusService> logger) // ✅ AGREGADO
        {
            _requestRepository = requestRepository;
            _documentRepository = documentRepository;
            _classificationRepository = classificationRepository;
            _exceptionRepository = exceptionRepository;
            _statusRepository = statusRepository;
            _unitOfWork = unitOfWork;
            _statusEvaluator = statusEvaluator;
            _logger = logger; // ✅ AGREGADO
        }

        /// <summary>
        /// /// 1. RecalculateStatus(int requestId)
        /// ------------------------------------------------------------
        /// Recalcula el estado global de una solicitud.
        /// 
        /// Flujo:
        /// - Obtiene la información básica de la solicitud.
        /// - Recupera documentos cargados, documentos requeridos y excepciones.
        /// - Filtra los documentos obligatorios (excluyendo excepciones "No aplica").
        /// - Evalúa condiciones clave:
        ///     • Si hay documentos observados
        ///     • Si todos están aprobados
        ///     • Si todos están cargados
        ///     • Si existen documentos faltantes
        ///     • Si la solicitud está vencida
        /// - Construye un contexto (StatusEvaluationContext).
        /// - Usa RequestStatusEvaluator (patrón Specification) para determinar el nuevo estado.
        /// - Actualiza el estado solo si cambió.
        ///
        /// Este método encapsula la lógica central del flujo de negocio del estado.
        ///
        /// ------------------------------------------------------------
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task RecalculateStatus(int requestId)
        {
            _logger.LogInformation("RecalculateStatus iniciado para RequestId={RequestId}", requestId);

            var request = await _requestRepository.GetStatusDataAsync(requestId);
            if (request is null)
            {
                _logger.LogWarning("RecalculateStatus: Solicitud no encontrada. RequestId={RequestId}", requestId);
                throw new Exception("Solicitud no encontrada");
            }

            IEnumerable<dynamic> documents;
            try
            {
                documents = await _documentRepository.GetActiveByRequestId(requestId);
                _logger.LogDebug("RecalculateStatus: Documentos activos obtenidos para RequestId={RequestId}", requestId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RecalculateStatus: FALLA EN GetActiveByRequestId documentos. RequestId={RequestId}", requestId);
                throw new Exception("FALLA EN: GetActiveByRequestId documentos", ex);
            }

            IEnumerable<dynamic> requiredDocs;
            try
            {
                requiredDocs = await _classificationRepository
                    .GetRequiredByClassification(request.IdAcquisitionClassification.Value);
                _logger.LogDebug("RecalculateStatus: Documentos requeridos obtenidos para ClassificationId={ClassificationId}", request.IdAcquisitionClassification.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RecalculateStatus: FALLA EN GetRequiredByClassification. RequestId={RequestId}", requestId);
                throw new Exception("FALLA EN: GetRequiredByClassification", ex);
            }

            IEnumerable<dynamic> exceptions;
            try
            {
                exceptions = await _exceptionRepository.GetActiveByRequestId(requestId);
                _logger.LogDebug("RecalculateStatus: Excepciones activas obtenidas para RequestId={RequestId}", requestId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RecalculateStatus: FALLA EN GetActiveByRequestId exceptions. RequestId={RequestId}", requestId);
                throw new Exception("FALLA EN: GetActiveByRequestId exceptions", ex);
            }

            var obligatorios = requiredDocs
                .Where(r =>
                    r.IsRequired &&
                    !exceptions.Any(e =>
                        e.IdDocumentType == r.DocumentTypeId &&
                        e.DoesNotApply &&
                        e.Active))
                .ToList();

            _logger.LogDebug("RecalculateStatus: Documentos obligatorios (sin excepciones)={Count} para RequestId={RequestId}", obligatorios.Count, requestId);

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
                             DateTime.UtcNow > request.CompleteMaximeDate.Value;

            var ctx = new StatusEvaluationContext(
                HasObservado: hasObservado,
                AllApproved: allApproved,
                AllLoaded: allLoaded,
                HasLoaded: hasLoaded,
                HasMissing: missingCount > 0,
                IsExpired: isExpired
            );

            var newStatus = _statusEvaluator.Evaluate(ctx);

            _logger.LogInformation(
                "RecalculateStatus: Evaluación completada. RequestId={RequestId} | StatusActual={StatusActual} | StatusNuevo={StatusNuevo} | HasObservado={HasObservado} | AllApproved={AllApproved} | AllLoaded={AllLoaded} | HasMissing={HasMissing} | IsExpired={IsExpired}",
                requestId, request.IdApplicationStatus, (int)newStatus, hasObservado, allApproved, allLoaded, missingCount > 0, isExpired);

            try
            {
                if (request.IdApplicationStatus != (int)newStatus)
                {
                    _logger.LogInformation("RecalculateStatus: Actualizando estado. RequestId={RequestId} | De={StatusAnterior} => A={StatusNuevo}", requestId, request.IdApplicationStatus, (int)newStatus);
                    await _requestRepository.UpdateStatusAsync(requestId, (int)newStatus);
                    _logger.LogInformation("RecalculateStatus: Estado actualizado exitosamente. RequestId={RequestId}", requestId);
                }
                else
                {
                    _logger.LogDebug("RecalculateStatus: Estado sin cambios, no se actualiza. RequestId={RequestId} | Status={Status}", requestId, (int)newStatus);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RecalculateStatus: FALLA EN UpdateStatusAsync. RequestId={RequestId}", requestId);
                throw new Exception("FALLA EN: UpdateStatusAsync", ex);
            }
        }

        /// <summary>
        /// ///
        /// 2. GetDocumentGroupStatus(int requestId)
        /// ------------------------------------------------------------
        /// Obtiene el estado agrupado por tipo de documento dentro de una solicitud.
        ///
        /// Flujo:
        /// - Obtiene documentos requeridos según clasificación.
        /// - Filtra documentos obligatorios (considerando excepciones).
        /// - Agrupa por tipo de documento.
        /// - Calcula métricas por grupo:
        ///     • Total requeridos
        ///     • Total cargados
        ///     • Total aprobados
        ///     • Total observados
        /// - Determina el estado del grupo según reglas:
        ///     • Pendiente → no hay documentos
        ///     • Observado → existe al menos uno observado
        ///     • Cargado → hay documentos cargados
        ///     • Completo → todos aprobados y completos
        /// - Retorna una lista con el estado de cada grupo.
        ///
        /// Este método se usa principalmente para UI o visualización del progreso.
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// 

        public async Task<List<DocumentGroupStatusDto>> GetDocumentGroupStatus(int requestId)
        {
            _logger.LogInformation("GetDocumentGroupStatus iniciado para RequestId={RequestId}", requestId);

            var request = await _requestRepository.GetByIdAsync(requestId);

            if (request == null)
            {
                _logger.LogWarning("GetDocumentGroupStatus: Solicitud no encontrada. RequestId={RequestId}", requestId);
                throw new Exception("Solicitud no encontrada.");
            }

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

                _logger.LogDebug(
                    "GetDocumentGroupStatus: DocumentTypeId={DocumentTypeId} | Requeridos={Required} | Subidos={Uploaded} | Aprobados={Approved} | Observados={Observed} | Estado={Status}",
                    group.Key, required, uploaded, approved, observed, status);

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

            _logger.LogInformation("GetDocumentGroupStatus: Completado. RequestId={RequestId} | GruposEvaluados={Count}", requestId, result.Count);

            return result;
        }
    }
}