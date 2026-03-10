using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Commands.DeletePaymentPolicy
{
    public record DeletePaymentPolicyCommand(
     int IdPaymentPolicy,
     string Password
 ) : IRequest<bool>;
}
