namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        /// Valida credenciales y genera el token de autenticación
        Task<LoginResponse> LoginAsync(User user, string Password);
    }
}
