

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Services.Auth
{
    public interface IAuthService
    {
        /// Valida credenciales y genera el token de autenticación
        Task<LoginResponse> LoginAsync(User user, string Password);
    }
}
