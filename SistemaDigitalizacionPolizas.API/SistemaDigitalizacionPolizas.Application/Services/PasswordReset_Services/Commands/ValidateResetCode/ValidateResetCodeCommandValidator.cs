using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.ValidateResetCode
{
    public class ValidateResetCodeCommandValidator
     : AbstractValidator<ValidateResetCodeCommand>
    {
        public ValidateResetCodeCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("Correo inválido.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("El código es obligatorio.")
                .Length(6).WithMessage("El código debe tener 6 dígitos.");
        }
    }
}
