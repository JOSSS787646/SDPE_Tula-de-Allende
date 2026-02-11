using SistemaDigitalizacionPolizas.Domain.Entities.Acquisition_Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition
{
    /// <summary>
    /// Repositorio para la clasificación de adquisiciones.
    /// </summary>
    public interface IAcquisitionClassificationRepository
    {
        /// <summary>Agrega una clasificación de adquisición.</summary>
        Task<AcquisitionClassification?> AddAsync(AcquisitionClassification unit);

        /// <summary>Obtiene una clasificación por Id.</summary>
        Task<AcquisitionClassification?> GetByIdAsync(int idAcquisitionClassification);

        /// <summary>Obtiene todas las clasificaciones.</summary>
        Task<IEnumerable<AcquisitionClassification>> GetAllAsync();

        /// <summary>Actualiza una clasificación.</summary>
        Task<bool> UpdateAsync(AcquisitionClassification unit);

        /// <summary>Desactiva una clasificación (soft delete).</summary>
        Task<bool> DeleteAsync(int code, int idUser);

        /// <summary>Obtiene una clasificación por código.</summary>
        Task<AcquisitionClassification?> GetByCodeAsync(int code);
    }
}
