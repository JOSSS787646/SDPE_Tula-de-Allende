using SistemaDigitalizacionPolizas.Domain.Dtos.PaymentPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Queries.GetAvailablePaymentPolicies
{
    public record GetAvailablePaymentPoliciesQuery()
      : IRequest<List<PaymentPolicySimpleDto>>;
}
