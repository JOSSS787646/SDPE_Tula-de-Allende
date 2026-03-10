using SistemaDigitalizacionPolizas.Domain.Entities.ApplicationStatus;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest
{
    public interface IApplicationStatusRepository
    {

        //Agregar un nuevo fondo
        Task<ApplicationStatus?> AddAsync(ApplicationStatus unit);
        //Obtener un fondo por su codigo
        Task<ApplicationStatus?> GetByCodeAsync(int code);
        //Obtener todos los fondos
        Task<IEnumerable<ApplicationStatus>> GetAllAsync();
        //Acctualizar la informacion de un fondo
        Task<bool> UpdateAsync(ApplicationStatus unit);
        //Desactivar un fondo
        Task<bool> DeleteAsync(int code, int userId);
        Task<ApplicationStatus?> GetByIdAsync(int id);
   
    }
}
