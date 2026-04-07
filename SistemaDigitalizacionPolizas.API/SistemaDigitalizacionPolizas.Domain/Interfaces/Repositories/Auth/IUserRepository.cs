

using SistemaDigitalizacionPolizas.Domain.Dtos.User;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Auth
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de usuarios.
    ///
    /// Permite realizar operaciones de consulta, creación y actualización
    /// de usuarios, así como la obtención de información relacionada con
    /// roles, permisos y datos de contacto.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Obtiene un usuario por su correo electrónico.
        /// </summary>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Obtiene un usuario junto con sus roles y permisos asociados.
        /// </summary>
        Task<User?> GetUserWithRolesAndPermissionsAsync(string email);

        /// <summary>
        /// Actualiza la fecha del último acceso del usuario.
        /// </summary>
        Task UpdateLastAccessAsync(int idUser);

        /// <summary>
        /// Crea un nuevo usuario en el sistema.
        /// </summary>
        Task AddAsync(User user);

        /// <summary>
        /// Actualiza la contraseña del usuario (ej. recuperación de contraseña).
        /// </summary>
        Task<bool> UpdatePasswordAsync(int idUser, string hashedPassword);

        /// <summary>
        /// Obtiene una lista paginada de usuarios.
        /// </summary>
        Task<List<UserListDto>> GetUsersAsync(int page, int pageSize);

        /// <summary>
        /// Obtiene un usuario por su identificador.
        /// </summary>
        Task<User?> GetByIdAsync(int idUser);

        /// <summary>
        /// Actualiza el estado del usuario (activo/inactivo).
        /// </summary>
        Task<bool> UpdateStatusAsync(int idUser, bool status);

        /// <summary>
        /// Actualiza los datos básicos del usuario como correo, rol y unidad administrativa.
        /// </summary>
        Task<bool> UpdateUserDataAsync(int idUser, string email, int idRole, int idAdministrativeUnit);

        /// <summary>
        /// Obtiene el correo de un usuario asociado a un rol específico.
        /// </summary>
        Task<string?> GetEmailByRoleAsync(int roleId);

        /// <summary>
        /// Obtiene el identificador de usuario asociado a un rol específico.
        /// </summary>
        Task<int> GetUserIdByRoleAsync(int roleId);

        /// <summary>
        /// Obtiene una lista de correos de usuarios asociados a un rol.
        /// </summary>
        Task<List<string>> GetEmailsByRoleAsync(int roleId);

        /// <summary>
        /// Obtiene una lista de identificadores de usuarios asociados a un rol.
        /// </summary>
        Task<List<int>> GetUserIdsByRoleAsync(int roleId);
    }
}
