namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Roles
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de roles del sistema.
    ///
    /// Permite realizar operaciones CRUD sobre los roles, incluyendo
    /// consultas por nombre e identificador.
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// Obtiene todos los roles registrados.
        /// </summary>
        Task<IEnumerable<Role>> GetAllAsync();

        /// <summary>
        /// Obtiene un rol mediante su nombre.
        /// </summary>
        Task<Role?> GetByNameAsync(string RolName);

        /// <summary>
        /// Obtiene un rol por su identificador.
        /// </summary>
        Task<Role?> GetByIdAsync(int idRol);

        /// <summary>
        /// Agrega un nuevo rol al sistema.
        /// </summary>
        Task AddAsync(Role role);

        /// <summary>
        /// Actualiza la información de un rol existente.
        /// </summary>
        Task<bool> UpdateAsync(Role role);

        /// <summary>
        /// Elimina un rol por su identificador.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
