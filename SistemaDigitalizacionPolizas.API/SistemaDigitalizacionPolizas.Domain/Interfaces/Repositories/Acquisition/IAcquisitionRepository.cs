using SistemaDigitalizacionPolizas.Domain.Entities.Acquisition_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.Actions_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition
{
    public interface IAcquisitionRepository
    {

        // Agrega una nueva acción al sistema
        Task<AcquisitionType?> AddAsync(AcquisitionType unit);

        // Obtiene una acción por su id técnico
        Task<AcquisitionType?> GetByIdAsync(int idAcquisitionType);

        // Obtiene todas las acciones registradas
        Task<IEnumerable<AcquisitionType>> GetAllAsync();

        // Actualiza la información de una acción existente
        Task<bool> UpdateAsync(AcquisitionType unit);

        // Desactiva una acción (eliminación lógica)
        Task<bool> DeleteAsync(int code, int idAcquisitionType);

        // Obtiene una acción por su código de negocio
        Task<AcquisitionType?> GetByCodeAsync(int code);

    }
}
