using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.UpdateExpedientDocument
{
    public class UpdateExpedientDocumentCommandHandler
       : IRequestHandler<UpdateExpedientDocumentCommand, bool>
    {
        private readonly IDocumentExpedientRepository _repository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IRequestStatusService _requestStatusService;
        private readonly IUnitOfWorkService _unitOfWork;

        public UpdateExpedientDocumentCommandHandler(
            IDocumentExpedientRepository repository,
            IFileStorageService fileStorageService,
            IRequestStatusService requestStatusService,
            IUnitOfWorkService unitOfWork)
        {
            _repository = repository;
            _fileStorageService = fileStorageService;
            _requestStatusService = requestStatusService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
    UpdateExpedientDocumentCommand request,
    CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            if (entity == null)
                throw new Exception("Documento no encontrado.");

            string? oldFilePath = entity.FilePath;
            string? newFilePath = null;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 🔥 Si viene nuevo archivo
                if (request.NewFile != null)
                {
                    await using var stream = request.NewFile.OpenReadStream();

                    newFilePath = await _fileStorageService.UploadAsync(
                        stream,
                        request.NewFile.FileName,
                        request.NewFile.ContentType,
                        "expedientes"
                    );

                    entity.FileName = request.NewFile.FileName;
                    entity.FilePath = newFilePath;
                    entity.UploadDate = DateTime.UtcNow;

                    // estado cargado automáticamente
                    entity.IdDocumentStatus = 2;
                }

                // 🔥 actualizar observaciones
                if (request.Observations != null)
                    entity.Observations = request.Observations;

                _repository.Update(entity);

                // 🔥 recalcular estado de la solicitud
                await _requestStatusService.RecalculateStatus(entity.RequestId);

                // 🔥 commit único
                await _unitOfWork.CommitAsync();

                // 🔥 eliminar archivo viejo si todo salió bien
                if (newFilePath != null && !string.IsNullOrEmpty(oldFilePath))
                    await _fileStorageService.DeleteAsync(oldFilePath);

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();

                // si falló BD eliminar archivo nuevo
                if (newFilePath != null)
                    await _fileStorageService.DeleteAsync(newFilePath);

                throw;
            }
        }
    }
}