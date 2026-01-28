using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.PasswordReset
{
    public record RequestPasswordResetCommand(string Email)
    : IRequest<bool>;
}
