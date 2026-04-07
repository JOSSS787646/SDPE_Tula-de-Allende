using SistemaDigitalizacionPolizas.Domain.Entities.Auth_Entities;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Auth
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de tokens
    /// de recuperación de contraseña.
    ///
    /// Permite crear, validar e invalidar tokens utilizados en el
    /// proceso de restablecimiento de contraseña.
    /// </summary>
    public interface IPasswordResetRepository
    {
        /// <summary>
        /// Crea y almacena un nuevo token de recuperación de contraseña.
        /// </summary>
        Task CreateAsync(PasswordResetToken token);

        /// <summary>
        /// Obtiene un token válido para un usuario específico,
        /// verificando que coincida con el código y no esté expirado o invalidado.
        /// </summary>
        Task<PasswordResetToken?> GetValidTokenAsync(int userId, string code);

        /// <summary>
        /// Invalida un token de recuperación, evitando su reutilización.
        /// </summary>
        Task InvalidateAsync(int id);
    }
}
