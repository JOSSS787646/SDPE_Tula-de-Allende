using Microsoft.Extensions.Logging;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;


namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service
{
    /// <summary>
    /// Servicio encargado de eliminar una solicitud junto con todos sus documentos
    /// y archivos asociados en almacenamiento externo.
    ///
    /// Realiza una eliminación en cascada controlada, asegurando que los archivos
    /// físicos se eliminen antes de remover la información en base de datos.
    /// </summary>
    public class RequestDeleteService : IRequestDeleteService
    {
        private readonly IDocumentExpedientRepository _documentRepository;
        private readonly IAcquisitionRequest _requestRepository;
        private readonly IFileStorageService _fileService;
        private readonly ILogger<RequestDeleteService> _logger;

        public RequestDeleteService(
            IDocumentExpedientRepository documentRepository,
            IAcquisitionRequest requestRepository,
            IFileStorageService fileService,
            ILogger<RequestDeleteService> logger)
        {
            _documentRepository = documentRepository;
            _requestRepository = requestRepository;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task DeleteRequestCascade(int requestId)
        {
            _logger.LogInformation("Iniciando eliminación en cascada de la solicitud {RequestId}", requestId);

            try
            {
                var documents = await _documentRepository.GetActiveByRequestId(requestId);

                foreach (var doc in documents)
                {
                    if (!string.IsNullOrEmpty(doc.FilePath))
                    {
                        try
                        {
                            await _fileService.DeleteFileAsync(doc.FilePath);
                            _logger.LogInformation("Archivo eliminado: {FilePath}", doc.FilePath);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex,
                                "Error al eliminar archivo {FilePath} de la solicitud {RequestId}",
                                doc.FilePath, requestId);
                        }
                    }
                }

                await _requestRepository.DeleteCascadeAsync(requestId);

                _logger.LogInformation("Solicitud {RequestId} eliminada correctamente", requestId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error al eliminar en cascada la solicitud {RequestId}",
                    requestId);

                throw;
            }
        }
    }
}