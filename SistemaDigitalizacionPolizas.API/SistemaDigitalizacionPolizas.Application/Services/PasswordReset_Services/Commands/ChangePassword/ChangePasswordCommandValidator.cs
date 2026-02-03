using FluentValidation;

namespace SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator
      : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.Data.Code)
                .NotEmpty().EmailAddress();

            RuleFor(x => x.Data.Code)
                .NotEmpty().Length(6);

            RuleFor(x => x.Data.NewPassword)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("La contraseña debe tener al menos 6 caracteres.");
        }
    }
}
