using SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Queries.GetRequiredDocumentsByRequest
{
    public class GetDocumentsByAcquisitionClassificationQueryHandler
         : IRequestHandler<GetDocumentsByAcquisitionClassificationQuery, IEnumerable<ExpedientDocumentPreviewDto>>
    {
        private readonly IDocumentExpedientRepository _repository;

        public GetDocumentsByAcquisitionClassificationQueryHandler(IDocumentExpedientRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ExpedientDocumentPreviewDto>> Handle(
            GetDocumentsByAcquisitionClassificationQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetDocumentsByClassificationAsync(request.AcquisitionClassificationId);
        }
    }
}
