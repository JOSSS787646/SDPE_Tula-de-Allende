using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Queries.GetDocumentsChecklistByRequest
{
    public class GetDocumentsChecklistByRequestHandler
        : IRequestHandler<GetDocumentsChecklistByRequestQuery, List<RequestDocumentChecklistDto>>
    {
        private readonly IDocumentExpedientRepository _repository;

        public GetDocumentsChecklistByRequestHandler(IDocumentExpedientRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RequestDocumentChecklistDto>> Handle(
            GetDocumentsChecklistByRequestQuery request,
            CancellationToken cancellationToken)
        {
            // Toda la lógica vive en el repositorio
            return await _repository.GetChecklistByRequestAsync(request.RequestId);
        }
    }
}
