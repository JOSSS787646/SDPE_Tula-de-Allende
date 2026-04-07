using MediatR;
using SistemaDigitalizacionPolizas.Domain.Constants;
using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Enums;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;


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
                // CALCULAR ESTADO GLOBAL
                // ================================
                if (!document.RequiredByRule || document.NoApplies)
                {
                    document.GlobalStatus = DocumentGlobalStatus.NoAplica;
                }
                else if (document.Files == null || !document.Files.Any())
                {
                    document.GlobalStatus = DocumentGlobalStatus.Pendiente;
                }
                else
                {
                    int uploaded = document.Files.Count;

                    int approved = document.Files.Count(f =>
                        f.Status != null &&
                        f.Status.Id == (int)DocumentStatusEnum.Aprobado);

                    int observed = document.Files.Count(f =>
                        f.Status != null &&
                        f.Status.Id == (int)DocumentStatusEnum.Observado);

                    int loaded = document.Files.Count(f =>
                        f.Status != null &&
                        f.Status.Id == (int)DocumentStatusEnum.Cargado);

                    if (observed > 0)
                    {
                        document.GlobalStatus = DocumentGlobalStatus.Observado;
                    }
                    else if (loaded > 0)
                    {
                        document.GlobalStatus = DocumentGlobalStatus.Cargado;
                    }
                    else if (approved == uploaded && uploaded > 0)
                    {
                        document.GlobalStatus = DocumentGlobalStatus.Completo;
                    }
                    else
                    {
                        document.GlobalStatus = DocumentGlobalStatus.Cargado;
                    }
                }

                // ================================
                // GENERAR PREVIEW URLs (SIN CAMBIOS)
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