namespace SistemaDigitalizacionPolizas.Domain.Dtos.Auth
{
    public class PasswordResetDto
    {
        public string Email { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}
