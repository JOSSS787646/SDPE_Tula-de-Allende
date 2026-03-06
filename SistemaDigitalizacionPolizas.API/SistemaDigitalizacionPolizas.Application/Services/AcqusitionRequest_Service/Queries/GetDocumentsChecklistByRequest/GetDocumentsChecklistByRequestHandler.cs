using MediatR;
using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Queries.GetDocumentsChecklistByRequest
{
    public class GetDocumentsChecklistByRequestHandler
        : IRequestHandler<GetDocumentsChecklistByRequestQuery, List<RequestDocumentChecklistDto>>
    {
        private readonly IDocumentExpedientRepository _repository;
        private readonly IFileStorageService _fileStorageService;

        public GetDocumentsChecklistByRequestHandler(
            IDocumentExpedientRepository repository,
            IFileStorageService fileStorageService)
        {
            _repository = repository;
            _fileStorageService = fileStorageService;
        }

        public async Task<List<RequestDocumentChecklistDto>> Handle(
     GetDocumentsChecklistByRequestQuery request,
     CancellationToken cancellationToken)
        {
            var result = await _repository.GetChecklistByRequestAsync(request.RequestId);

            foreach (var doc in result)
            {
                if (doc.FileUrls != null && doc.FileUrls.Any())
                {
                    doc.PreviewUrls = doc.FileUrls
                        .Select(x => _fileStorageService.GetPresignedUrl(x, 10))
                        .ToList();
                }
            }

            return result;
        }
    }
}