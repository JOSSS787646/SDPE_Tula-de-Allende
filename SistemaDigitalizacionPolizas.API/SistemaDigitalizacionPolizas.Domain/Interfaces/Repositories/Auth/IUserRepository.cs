

using SistemaDigitalizacionPolizas.Domain.Dtos.User;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Auth
{
    public interface IUserRepository
    {
        /// Obtiene un usuario por su correo electrónico
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetUserWithRolesAndPermissionsAsync(string email);
        /// Actualiza la fecha del último acceso
        Task UpdateLastAccessAsync(int idUser);
        //Crear a un Usuario
        Task AddAsync(User user);
        // Actualiza la contraseña de recuperacion
        Task<bool> UpdatePasswordAsync(int idUser, string hashedPassword);
        //Obtiene todos los usuarios
        Task<List<UserListDto>> GetUsersAsync(int page, int pageSize);
        Task<User?> GetByIdAsync(int idUser);
        Task<bool> UpdateStatusAsync(int idUser, bool status);

        //Actualiza los datos del usuario
        Task<bool> UpdateUserDataAsync(int idUser, string email, int idRole, int idAdministrativeUnit);

        Task<string?> GetEmailByRoleAsync(int roleId);

        Task<int> GetUserIdByRoleAsync(int roleId);

    }
}
