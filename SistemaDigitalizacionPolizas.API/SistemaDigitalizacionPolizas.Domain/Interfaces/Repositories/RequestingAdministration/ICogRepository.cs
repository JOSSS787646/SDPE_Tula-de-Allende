using SistemaDigitalizacionPolizas.Domain.Entities.Areas_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de COG (Catálogo de Objetos de Gasto).
    ///
    /// Permite realizar operaciones CRUD sobre los registros del catálogo,
    /// incluyendo consultas por identificador y código, así como eliminación lógica.
    /// </summary>
    public interface ICogRepository
    {
        /// <summary>
        /// Agrega un nuevo registro al catálogo COG.
        /// </summary>
        Task<COG?> AddAsync(COG unit);

        /// <summary>
        /// Obtiene un registro COG mediante su código.
        /// </summary>
        Task<COG?> GetByCodeAsync(int code);

        /// <summary>
        /// Obtiene todos los registros del catálogo COG.
        /// </summary>
        Task<IEnumerable<COG>> GetAllAsync();

        /// <summary>
        /// Actualiza la información de un registro COG existente.
        /// </summary>
        Task<bool> UpdateAsync(COG unit);

        /// <summary>
        /// Realiza una eliminación lógica del registro COG,
        /// registrando el usuario que ejecuta la acción.
        /// </summary>
        Task<bool> DeleteAsync(int code, int userId);

        /// <summary>
        /// Obtiene un registro COG por su identificador.
        /// </summary>
        Task<COG?> GetByIdAsync(int id);
    }
}
