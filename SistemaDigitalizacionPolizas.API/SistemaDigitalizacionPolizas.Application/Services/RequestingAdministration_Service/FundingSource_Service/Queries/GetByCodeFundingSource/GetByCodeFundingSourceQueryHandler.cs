using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Queries.GetByCodeFundingSource
{
    public class GetByCodeFundingSourceQueryHandler
        : IRequestHandler<GetByCodeFundingSourceQuery, FundingSourceDto?>
    {

        private readonly IFundingSourceRepository _repository;

        public GetByCodeFundingSourceQueryHandler(IFundingSourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<FundingSourceDto?> Handle(
            GetByCodeFundingSourceQuery request,
            CancellationToken cancellationToken)
        {
            var fundingSource = await _repository.GetByCodeAsync(request.code);
            if (fundingSource == null)
            {
                return null;
            }
            return new FundingSourceDto
            {
                idFundingSource = fundingSource.idFundingSource,
                Code = fundingSource.Code,
                Description = fundingSource.Description,
                Active = fundingSource.Active
            };
        }
    }
}
