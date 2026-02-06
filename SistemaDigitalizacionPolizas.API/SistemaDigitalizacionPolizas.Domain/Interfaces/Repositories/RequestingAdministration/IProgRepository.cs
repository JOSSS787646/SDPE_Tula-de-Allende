using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration
{
    public interface IProgRepository
    {
        //Agregar un nuevo Prog
        Task<Prog?> AddAsync(Prog unit);
        //Obtener un Prog por su codigo
        Task<Prog?> GetByCodeAsync(int code);
        //Obtener todos los Prog
        Task<IEnumerable<Prog>> GetAllAsync();
        //Actualizar la informacion de un Prog
        Task<bool> UpdateAsync(Prog unit);
        //Desactivar un Prog
        Task<bool> DeleteAsync(int code, int userId);
    }
}
