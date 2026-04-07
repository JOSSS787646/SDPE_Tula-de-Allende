using SistemaDigitalizacionPolizas.Domain.Entities.Areas_Entities;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.AdministrtiveUnit
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de unidades administrativas.
    ///
    /// Permite realizar operaciones CRUD sobre las unidades administrativas,
    /// incluyendo consultas por Id y código, así como eliminación lógica.
    /// </summary>
    public interface IAdministrativeUnit
    {
        /// <summary>
        /// Crea una nueva unidad administrativa.
        /// </summary>
        Task<AdministrativeUnit?> CreateAsync(AdministrativeUnit unit);

        /// <summary>
        /// Obtiene una unidad administrativa mediante su código.
        /// </summary>
        Task<AdministrativeUnit?> GetByCodeAsync(int code);

        /// <summary>
        /// Obtiene todas las unidades administrativas registradas.
        /// </summary>
        Task<IEnumerable<AdministrativeUnit>> GetAllAsync();

        /// <summary>
        /// Actualiza la información de una unidad administrativa existente.
        /// </summary>
        Task<bool> UpdateAsync(AdministrativeUnit id);

        /// <summary>
        /// Realiza una eliminación lógica de la unidad administrativa,
        /// registrando el usuario que ejecuta la acción.
        /// </summary>
        Task<bool> DeleteAsync(int code, int userId);

        /// <summary>
        /// Obtiene una unidad administrativa por su identificador.
        /// </summary>
        Task<AdministrativeUnit?> GetByIdAsync(int id);
    }
}
