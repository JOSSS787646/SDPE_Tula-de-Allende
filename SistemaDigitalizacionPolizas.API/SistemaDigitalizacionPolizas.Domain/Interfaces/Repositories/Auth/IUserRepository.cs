

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

    }
}
