using MediatR;
using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.CreateExpedientDocument
{
    public class CreateExpedientDocumentCommandHandler
       : IRequestHandler<CreateExpedientDocumentCommand, int>
    {
        private readonly IDocumentExpedientRepository _repository;
        private readonly IDocumentStatusRepository _documentStatusRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly ICurrentUserService _currentUserService;

        public CreateExpedientDocumentCommandHandler(
            IDocumentExpedientRepository repository,
            IDocumentStatusRepository documentStatusRepository,
            IFileStorageService fileStorageService,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _documentStatusRepository = documentStatusRepository;
            _fileStorageService = fileStorageService;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(
            CreateExpedientDocumentCommand request,
            CancellationToken cancellationToken)
        {
            string filePath = string.Empty;

            try
            {
                // ================================
                // 1️⃣ Subir archivo a Wasabi
                // ================================
                using var stream = request.File.OpenReadStream();

                filePath = await _fileStorageService.UploadAsync(
                    stream,
                    request.File.FileName,
                    request.File.ContentType,
                    "expedientes"
                );

                var userId = _currentUserService.UserId;

                // ================================
                // 2️⃣ Resolver estado de documento
                // ================================
                int? idEstadoDocumento = request.IdDocumentStatus;

                if (idEstadoDocumento == null)
                {
                    // Ejemplo: Code = 1 => PENDIENTE o CARGADO
                    var defaultStatus = await _documentStatusRepository.GetByCodeAsync(1);
                    if (defaultStatus == null)
                        throw new InvalidOperationException("No existe un estado de documento por defecto (Code = 1).");

                    idEstadoDocumento = defaultStatus.idDocumentStatus;
                }

                // ================================
                // 3️⃣ Crear entidad
                // ================================
                var entity = new ExpedientDocument
                {
                    RequestId = request.RequestId,
                    DocumentTypeId = request.DocumentTypeId,
                    IdDocumentStatus = idEstadoDocumento,

                    FileName = request.File.FileName,
                    FilePath = filePath,
                    UploadDate = DateTime.UtcNow,

                    Observations = request.Observations,

                    UploadedBy = userId,
                    CreatedBy = userId,
                    CreatedAt = DateTime.UtcNow,

                    Active = true
                };

                await _repository.AddAsync(entity);
                await _repository.SaveChangesAsync();

                return entity.Id;
            }
            catch
            {
                // 🔥 Rollback del archivo si falla la BD
                if (!string.IsNullOrEmpty(filePath))
                {
                    await _fileStorageService.DeleteAsync(filePath);
                }

                throw;
            }
        }
    }
}