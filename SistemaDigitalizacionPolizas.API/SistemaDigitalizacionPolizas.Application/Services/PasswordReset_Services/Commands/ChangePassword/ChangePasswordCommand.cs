namespace SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.ChangePassword
{
    public record ChangePasswordCommand(ResetPasswordDto Data)
         : IRequest<bool>;
}
