using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.CreateExpedientDocument
{
    public class CreateExpedientDocumentCommandHandler
       : IRequestHandler<CreateExpedientDocumentCommand, int>
    {
        private readonly IDocumentExpedientRepository _repository;
        private readonly IFileStorageService _fileStorageService;
        private readonly ICurrentUserService _currentUserService;

        public CreateExpedientDocumentCommandHandler(
            IDocumentExpedientRepository repository,
            IFileStorageService fileStorageService,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
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

                var userId = _currentUserService.UserId; // 👈 clave

                // ================================
                // 2️⃣ Crear entidad con auditoría
                // ================================
                var entity = new ExpedientDocument
                {
                    RequestId = request.RequestId,
                    DocumentTypeId = request.DocumentTypeId,

                    FileName = request.File.FileName,
                    FilePath = filePath,
                    UploadDate = DateTime.UtcNow,

                    DocumentStatus = request.DocumentStatus ?? "CARGADO",
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
                if (!string.IsNullOrEmpty(filePath))
                {
                    await _fileStorageService.DeleteAsync(filePath);
                }

                throw;
            }
        }
    }
}
