using SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Queries.DownloadDocument
{
    public class DownloadDocumentQueryHandler
       : IRequestHandler<DownloadDocumentQuery, DownloadDocumentDto>
    {
        private readonly IDocumentExpedientRepository _repository;
        private readonly IFileStorageService _fileStorageService;

        public DownloadDocumentQueryHandler(
            IDocumentExpedientRepository repository,
            IFileStorageService fileStorageService)
        {
            _repository = repository;
            _fileStorageService = fileStorageService;
        }

        public async Task<DownloadDocumentDto> Handle(
            DownloadDocumentQuery request,
            CancellationToken cancellationToken)
        {
            var document = await _repository.GetByIdAsync(request.DocumentId);

            if (document == null || string.IsNullOrEmpty(document.FilePath))
                throw new Exception("Documento no encontrado");

            var file = await _fileStorageService.GetFileAsync(document.FilePath);

            return new DownloadDocumentDto(
                document.FileName!,
                file.Headers.ContentType,
                file.ResponseStream
            );
        }
    }
}
