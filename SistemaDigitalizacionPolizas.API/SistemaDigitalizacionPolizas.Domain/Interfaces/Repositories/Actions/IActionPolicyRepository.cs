using SistemaDigitalizacionPolizas.Domain.Entities.Actions_Entities;
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
        //Agregar un nuevo fondo
        Task<ActionsPolicy?> AddAsync(ActionsPolicy unit);
        //Obtener un fondo por su codigo
        Task<ActionsPolicy?> GetByCodeAsync(int code);
        //Obtener todos los fondos
        Task<IEnumerable<ActionsPolicy>> GetAllAsync();
        //Acctualizar la informacion de un fondo
        Task<bool> UpdateAsync(ActionsPolicy unit);
        //Desactivar un fondo
        Task<bool> DeleteAsync(int code, int userId);
    }
}
