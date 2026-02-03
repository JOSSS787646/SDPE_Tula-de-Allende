namespace SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.PasswordReset
{
    public record RequestPasswordResetCommand(string Email)
    : IRequest<bool>;
}
