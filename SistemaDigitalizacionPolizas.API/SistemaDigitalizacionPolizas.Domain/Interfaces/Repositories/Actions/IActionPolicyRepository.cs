using SistemaDigitalizacionPolizas.Domain.Entities.Actions_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.Areas_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Actions
{
    public interface IActionPolicyRepository
    {
        // Agrega una nueva acción al sistema
        Task<ActionsPolicy?> AddAsync(ActionsPolicy unit);

        // Obtiene una acción por su id técnico
        Task<ActionsPolicy?> GetByIdAsync(int id);

        // Obtiene todas las acciones registradas
        Task<IEnumerable<ActionsPolicy>> GetAllAsync();

        // Actualiza la información de una acción existente
        Task<bool> UpdateAsync(ActionsPolicy unit);

        // Desactiva una acción (eliminación lógica)
        Task<bool> DeleteAsync(int code, int userId);

        // Obtiene una acción por su código de negocio
        Task<ActionsPolicy?> GetByCodeAsync(int code);




    }
}
