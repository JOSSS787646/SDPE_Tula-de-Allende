using SistemaDigitalizacionPolizas.Domain.Dtos.PaymentPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Queries.DownloadPaymentPolicy
{
    public record DownloadPaymentPolicyQuery(
      int PaymentPolicyId
  ) : IRequest<DownloadPaymentPolicyDto>;
}
