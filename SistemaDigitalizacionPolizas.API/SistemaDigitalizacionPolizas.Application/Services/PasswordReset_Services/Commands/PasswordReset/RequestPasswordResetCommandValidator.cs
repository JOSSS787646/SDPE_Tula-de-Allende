using FluentValidation;

namespace SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.PasswordReset
{
    public class RequestPasswordResetCommandValidator
        : AbstractValidator<RequestPasswordResetCommand>
    {
        public RequestPasswordResetCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("El correo no es válido.");
        }
    }
}
