
namespace SistemaDigitalizacionPolizas.Domain.Dtos.Auth
{
    public class JwtResult
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
