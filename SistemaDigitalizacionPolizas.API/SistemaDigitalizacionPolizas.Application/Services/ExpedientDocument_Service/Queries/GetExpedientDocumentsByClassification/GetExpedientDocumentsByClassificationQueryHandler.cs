using SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Queries.GetExpedientDocumentsByClassification
{
    public class GetExpedientDocumentsByClassificationQueryHandler
        : IRequestHandler<GetExpedientDocumentsByClassificationQuery, List<ExpedientDocumentDto>>
    {
        private readonly IDocumentExpedientRepository _repository;

        public GetExpedientDocumentsByClassificationQueryHandler(
            IDocumentExpedientRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ExpedientDocumentDto>> Handle(
            GetExpedientDocumentsByClassificationQuery request,
            CancellationToken cancellationToken)
        {
            // 🔥 Llamada al repo paginado
            var (documents, total) = await _repository.GetByClassificationAsync(
                request.ClassificationId,
                request.Page,
                request.PageSize
            );

            // 🔁 Mapeo a DTO
            return documents.Select(doc => new ExpedientDocumentDto
            {
                Id = doc.Id,
                FileName = doc.FileName,
                FilePath = doc.FilePath,
                UploadDate = doc.UploadDate,
                Active = doc.Active,

                DocumentTypeId = doc.DocumentTypeId,
                DocumentTypeName = doc.DocumentType?.DocumentName,

                DocumentStatusId = doc.IdDocumentStatus,
                DocumentStatusDescription = doc.DocumentStatus?.Description
            }).ToList();
        }
    }
    }
