namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Auth
{
    public interface IJwtService
    {
        JwtResult GenerateToken(User user);
    }
}
