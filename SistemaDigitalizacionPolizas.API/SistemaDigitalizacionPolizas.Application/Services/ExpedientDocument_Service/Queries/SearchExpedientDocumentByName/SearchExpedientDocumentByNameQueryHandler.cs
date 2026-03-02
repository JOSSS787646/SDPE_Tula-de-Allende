using SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument;
using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Queries.SearchExpedientDocumentByName
{
    public class SearchExpedientDocumentByNameQueryHandler
     : IRequestHandler<SearchExpedientDocumentByNameQuery, List<ExpedientDocumentSearchDto>>
    {
        private readonly IDocumentExpedientRepository _repository;

        public SearchExpedientDocumentByNameQueryHandler(
            IDocumentExpedientRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ExpedientDocumentSearchDto>> Handle(
            SearchExpedientDocumentByNameQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.SearchByNameAsync(
                request.RequestId,
                request.FileName
            );
        }
    }
}
