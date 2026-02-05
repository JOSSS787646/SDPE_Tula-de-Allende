using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Queries.GetAllCog;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Queries.GetAllFundingSource
{
    public class GetAllFundingSourceQueryHandler
         : IRequestHandler<GetAllFundingSourceQuery, List<FundingSourceDto>>
    {
        private readonly IFundingSourceRepository _repository;



        public GetAllFundingSourceQueryHandler(IFundingSourceRepository repository)
        {
            _repository = repository;
        }



        public async Task<List<FundingSourceDto>> Handle(
            GetAllFundingSourceQuery request,
            CancellationToken cancellationToken)
        {
            var fundingSources = await _repository.GetAllAsync();
            return fundingSources.Select(r => new FundingSourceDto
            {
                
                Code = r.Code,
                Description = r.Description,
                Active = r.Active
            }).ToList();
        }
    }
}
