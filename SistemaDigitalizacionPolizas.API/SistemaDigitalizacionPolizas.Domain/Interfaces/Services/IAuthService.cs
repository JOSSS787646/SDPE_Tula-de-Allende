namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Services
{

    /// <summary>
    /// Interfaz que define el servicio de autenticación del sistema.
    ///
    /// Proporciona el método para validar credenciales de un usuario
    /// y generar un token de acceso (JWT) en caso de autenticación exitosa.
    ///
    /// Se utiliza para desacoplar la lógica de autenticación de su implementación.
    /// </summary>
    /// 

    public interface IAuthService
    {
        /// Valida credenciales y genera el token de autenticación
        Task<LoginResponse> LoginAsync(User user, string Password);
    }
}
