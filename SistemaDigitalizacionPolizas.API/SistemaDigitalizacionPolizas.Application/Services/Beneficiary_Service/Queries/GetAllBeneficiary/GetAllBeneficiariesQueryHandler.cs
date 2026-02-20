using SistemaDigitalizacionPolizas.Domain.Dtos.Beneficiary;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Queries.GetAllBeneficiary
{
    public class GetAllBeneficiariesQueryHandler
     : IRequestHandler<GetAllBeneficiariesQuery, List<BeneficiaryDto>>
    {
        private readonly IBeneficiaryRepository _repository;

        public GetAllBeneficiariesQueryHandler(IBeneficiaryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<BeneficiaryDto>> Handle(
            GetAllBeneficiariesQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
