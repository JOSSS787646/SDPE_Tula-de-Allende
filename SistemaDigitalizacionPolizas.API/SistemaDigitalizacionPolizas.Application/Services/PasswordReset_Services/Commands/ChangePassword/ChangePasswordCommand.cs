using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.ChangePassword
{
    public record ChangePasswordCommand(ResetPasswordDto Data)
         : IRequest<bool>;
}
