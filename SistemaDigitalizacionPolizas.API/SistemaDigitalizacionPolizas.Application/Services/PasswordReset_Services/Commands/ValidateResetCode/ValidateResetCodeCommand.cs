using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.ValidateResetCode
{
    public record ValidateResetCodeCommand(
       string Email,
       string Code
   ) : IRequest<bool>;
}
