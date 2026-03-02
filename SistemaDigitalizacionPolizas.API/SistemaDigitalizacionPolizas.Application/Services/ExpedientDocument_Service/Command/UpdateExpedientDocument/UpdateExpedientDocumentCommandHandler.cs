using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.UpdateExpedientDocument
{
    public class UpdateExpedientDocumentCommandHandler
       : IRequestHandler<UpdateExpedientDocumentCommand, bool>
    {
        private readonly IDocumentExpedientRepository _repository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWorkService _unitOfWork;

        public UpdateExpedientDocumentCommandHandler(
            IDocumentExpedientRepository repository,
            IFileStorageService fileStorageService,
            IUnitOfWorkService unitOfWork)
        {
            _repository = repository;
            _fileStorageService = fileStorageService;
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

            // 🔥 Si viene nuevo archivo
            if (request.NewFile != null)
            {
                await using var stream = request.NewFile.OpenReadStream();

                var newPath = await _fileStorageService.UploadAsync(
                    stream,
                    request.NewFile.FileName,
                    request.NewFile.ContentType,
                    "expedientes"
                );

                entity.FileName = request.NewFile.FileName;
                entity.FilePath = newPath;
                entity.UploadDate = DateTime.UtcNow;
            }

            // 🔥 Actualizar solo campos permitidos
            if (request.Observations != null)
                entity.Observations = request.Observations;

            if (request.IdDocumentStatus.HasValue)
                entity.IdDocumentStatus = request.IdDocumentStatus.Value;

            _repository.Update(entity);

            await _unitOfWork.CommitAsync();

            // 🔥 Eliminar archivo viejo SOLO si todo salió bien
            if (request.NewFile != null && !string.IsNullOrEmpty(oldFilePath))
            {
                await _fileStorageService.DeleteAsync(oldFilePath);
            }

            return true;
        }
    }
}
