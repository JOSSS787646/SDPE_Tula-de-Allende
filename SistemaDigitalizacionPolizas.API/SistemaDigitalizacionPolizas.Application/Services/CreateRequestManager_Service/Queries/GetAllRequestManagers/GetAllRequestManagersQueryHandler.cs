using SistemaDigitalizacionPolizas.Domain.Dtos.RequestManager;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.CreateRequestManager_Service.Queries.GetAllRequestManagers
{
    public class GetAllRequestManagersQueryHandler
        : IRequestHandler<GetAllRequestManagersQuery, IEnumerable<RequestManagerPreviewDto>>
    {
        private readonly IRequestManagerRepository _repository;

        public GetAllRequestManagersQueryHandler(IRequestManagerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<RequestManagerPreviewDto>> Handle(
            GetAllRequestManagersQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
