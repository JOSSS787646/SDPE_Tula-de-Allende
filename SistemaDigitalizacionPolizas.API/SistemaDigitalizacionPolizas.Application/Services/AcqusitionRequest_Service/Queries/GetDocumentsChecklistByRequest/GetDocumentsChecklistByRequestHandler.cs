using MediatR;
using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Enums;
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

            foreach (var document in result)
            {
                // ================================
                // 🔥 CALCULAR ESTADO GLOBAL
                // ================================
                if (!document.RequiredByRule || document.NoApplies)
                {
                    document.GlobalStatus = "No aplica";
                }
                else if (document.Files == null || !document.Files.Any())
                {
                    document.GlobalStatus = "Pendiente";
                }
                else
                {
                    int uploaded = document.Files.Count;

                    int approved = document.Files.Count(f =>
                        f.Status != null &&
                        f.Status.Id == (int)DocumentStatusEnum.Aprobado);

                    if (approved == uploaded && uploaded > 0)
                    {
                        document.GlobalStatus = "Completo";
                    }
                    else
                    {
                        document.GlobalStatus = "Cargado";
                    }
                }

                // ================================
                // 🔥 TU LÓGICA ACTUAL (NO TOCADA)
                // ================================
                if (document.Files != null && document.Files.Any())
                {
                    foreach (var file in document.Files)
                    {
                        file.PreviewUrl = _fileStorageService
                            .GetPresignedUrl(file.FileUrl, 10);
                    }
                }
            }

            return result;
        }
    }
}