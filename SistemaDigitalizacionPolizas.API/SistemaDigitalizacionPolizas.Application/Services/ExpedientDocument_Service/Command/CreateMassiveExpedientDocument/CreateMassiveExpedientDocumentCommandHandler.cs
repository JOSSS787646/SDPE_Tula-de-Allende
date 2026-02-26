using MediatR;
using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.CreateMassiveExpedientDocument
{
    public class CreateMassiveExpedientDocumentCommandHandler
        : IRequestHandler<CreateMassiveExpedientDocumentCommand, List<int>>
    {
        private readonly IDocumentExpedientRepository _repository;
        private readonly IDocumentStatusRepository _statusRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWorkService _unitOfWork;

        public CreateMassiveExpedientDocumentCommandHandler(
            IDocumentExpedientRepository repository,
            IDocumentStatusRepository statusRepository,
            IFileStorageService fileStorageService,
            ICurrentUserService currentUserService,
            IUnitOfWorkService unitOfWork)
        {
            _repository = repository;
            _statusRepository = statusRepository;
            _fileStorageService = fileStorageService;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<int>> Handle(
            CreateMassiveExpedientDocumentCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Documents == null || !request.Documents.Any())
                throw new Exception("Debe enviar al menos un archivo.");

            if (request.Documents.Count > 100)
                throw new Exception("Máximo 100 archivos permitidos por carga.");

            var uploadedPaths = new List<string>();
            var entities = new List<ExpedientDocument>();
            var userId = _currentUserService.UserId;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 🔥 Obtener estado por defecto UNA sola vez
                var defaultStatus = await _statusRepository.GetByCodeAsync(1);
                if (defaultStatus == null)
                    throw new Exception("No existe estado de documento por defecto.");

                foreach (var item in request.Documents)
                {
                    if (item.File == null || item.File.Length == 0)
                        throw new Exception("Uno de los archivos enviados es inválido.");

                    await using var stream = item.File.OpenReadStream();

                    var filePath = await _fileStorageService.UploadAsync(
                        stream,
                        item.File.FileName,
                        item.File.ContentType,
                        "expedientes"
                    );

                    uploadedPaths.Add(filePath);

                    entities.Add(new ExpedientDocument
                    {
                        RequestId = request.RequestId,
                        DocumentTypeId = item.DocumentTypeId,
                        IdDocumentStatus = item.IdDocumentStatus ?? defaultStatus.idDocumentStatus,

                        FileName = item.File.FileName,
                        FilePath = filePath,
                        UploadDate = DateTime.UtcNow,
                        Observations = item.Observations,

                        UploadedBy = userId,
                        CreatedBy = userId,
                        CreatedAt = DateTime.UtcNow,
                        Active = true
                    });
                }

                await _repository.AddRangeAsync(entities);

                await _unitOfWork.CommitAsync();

                return entities.Select(x => x.Id).ToList();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();

                // 🔥 Rollback físico en Wasabi
                foreach (var path in uploadedPaths)
                {
                    await _fileStorageService.DeleteAsync(path);
                }

                throw;
            }
        }
    }
}