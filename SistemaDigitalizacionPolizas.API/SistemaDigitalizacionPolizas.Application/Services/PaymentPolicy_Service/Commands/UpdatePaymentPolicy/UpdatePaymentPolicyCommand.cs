using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Commands.UpdatePaymentPolicy
{
    public record UpdatePaymentPolicyCommand(
      int IdPaymentPolicy,
      string PolicyCode,
      string Description,
      IFormFile? File
  ) : IRequest<bool>;
}
