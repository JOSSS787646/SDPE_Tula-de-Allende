using SistemaDigitalizacionPolizas.Domain.Entities.Acquisition_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de tipos de adquisición.
    ///
    /// Permite realizar operaciones CRUD sobre las acciones o tipos de adquisición,
    /// incluyendo consultas por identificador técnico y código de negocio.
    ///
    /// Implementa un esquema de eliminación lógica (soft delete).
    /// </summary>
    public interface IAcquisitionRepository
    {
        /// <summary>
        /// Agrega un nuevo tipo de adquisición.
        /// </summary>
        Task<AcquisitionType?> AddAsync(AcquisitionType unit);

        /// <summary>
        /// Obtiene un tipo de adquisición por su identificador.
        /// </summary>
        Task<AcquisitionType?> GetByIdAsync(int idAcquisitionType);

        /// <summary>
        /// Obtiene todos los tipos de adquisición registrados.
        /// </summary>
        Task<IEnumerable<AcquisitionType>> GetAllAsync();

        /// <summary>
        /// Actualiza la información de un tipo de adquisición existente.
        /// </summary>
        Task<bool> UpdateAsync(AcquisitionType unit);

        /// <summary>
        /// Realiza una eliminación lógica del tipo de adquisición.
        /// </summary>
        Task<bool> DeleteAsync(int code, int idAcquisitionType);

        /// <summary>
        /// Obtiene un tipo de adquisición mediante su código de negocio.
        /// </summary>
        Task<AcquisitionType?> GetByCodeAsync(int code);
    }
}
