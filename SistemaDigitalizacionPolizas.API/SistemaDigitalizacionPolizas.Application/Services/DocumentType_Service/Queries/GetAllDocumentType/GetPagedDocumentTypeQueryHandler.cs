using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Queries.GetAllFundingSource;
using SistemaDigitalizacionPolizas.Domain.Dtos.DocumentType;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentType_Service.Queries.GetAllDocumentType
{
    public class GetPagedDocumentTypeQueryHandler
        : IRequestHandler<GetPagedDocumentTypeQuery, IEnumerable<DocumentTypeDto>>
    {
        private readonly IDocumentTypeRepository _repository;

        public GetPagedDocumentTypeQueryHandler(IDocumentTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DocumentTypeDto>> Handle(
            GetPagedDocumentTypeQuery request,
            CancellationToken cancellationToken)
        {
            var documents = await _repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize);

            return documents.Select(d => new DocumentTypeDto
            {
                IdDocumentType = d.IdDocumentType,
                DocumentName = d.DocumentName,
                Description = d.Description,
                Active = d.Active
            });
        }
    }
}
