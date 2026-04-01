using SistemaDigitalizacionPolizas.Domain.Entities.Acquisition_Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de clasificaciones
    /// de adquisiciones.
    ///
    /// Permite realizar operaciones CRUD sobre las clasificaciones utilizadas
    /// en las solicitudes, incluyendo consultas por Id y código.
    ///
    /// Implementa un esquema de eliminación lógica (soft delete).
    /// </summary>
    public interface IAcquisitionClassificationRepository
    {
        /// <summary>
        /// Agrega una nueva clasificación de adquisición.
        /// </summary>
        Task<AcquisitionClassification?> AddAsync(AcquisitionClassification unit);

        /// <summary>
        /// Obtiene una clasificación por su identificador.
        /// </summary>
        Task<AcquisitionClassification?> GetByIdAsync(int idAcquisitionClassification);

        /// <summary>
        /// Obtiene todas las clasificaciones registradas.
        /// </summary>
        Task<IEnumerable<AcquisitionClassification>> GetAllAsync();

        /// <summary>
        /// Actualiza los datos de una clasificación existente.
        /// </summary>
        Task<bool> UpdateAsync(AcquisitionClassification unit);

        /// <summary>
        /// Realiza una eliminación lógica de la clasificación,
        /// registrando el usuario que ejecuta la acción.
        /// </summary>
        Task<bool> DeleteAsync(int code, int idUser);

        /// <summary>
        /// Obtiene una clasificación mediante su código.
        /// </summary>
        Task<AcquisitionClassification?> GetByCodeAsync(int code);
    }
}
