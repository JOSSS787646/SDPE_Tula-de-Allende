using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.UpdatePaymentPolicy
{
    public record UpdatePaymentPolicyCommand(
        int IdRequest,
        int IdPaymentPolicy
    ) : IRequest<bool>;
}
