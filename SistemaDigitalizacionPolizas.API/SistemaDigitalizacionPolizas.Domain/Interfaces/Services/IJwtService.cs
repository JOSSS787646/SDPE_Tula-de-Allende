namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Services
{
    /// <summary>
    /// Interfaz que define el servicio para la generación de tokens JWT.
    ///
    /// Se encarga de crear tokens de autenticación a partir de la información
    /// del usuario, incluyendo sus datos y roles para control de acceso.
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Genera un token JWT para el usuario especificado.
        /// </summary>
        JwtResult GenerateToken(User user);
    }
}
