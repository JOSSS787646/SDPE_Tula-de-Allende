using SistemaDigitalizacionPolizas.Domain.Dtos.PaymentPolicy;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IPaymentPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Queries.GetAvailablePaymentPolicies
{
    public class GetAvailablePaymentPoliciesQueryHandler
    : IRequestHandler<GetAvailablePaymentPoliciesQuery, List<PaymentPolicySimpleDto>>
    {
        private readonly IPaymentPolicyRepository _repository;

        public GetAvailablePaymentPoliciesQueryHandler(
            IPaymentPolicyRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PaymentPolicySimpleDto>> Handle(
            GetAvailablePaymentPoliciesQuery request,
            CancellationToken cancellationToken)
        {
            var policies = await _repository.GetAvailablePoliciesAsync();

            return policies.Select(x => new PaymentPolicySimpleDto
            {
                IdPaymentPolicy = x.IdPaymentPolicy,
                PolicyCode = x.PolicyCode,
                Description = x.Description,

            }).ToList();
        }
    }
}