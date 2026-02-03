namespace SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.ValidateResetCode
{
    public record ValidateResetCodeCommand(
       string Email,
       string Code
   ) : IRequest<bool>;
}
