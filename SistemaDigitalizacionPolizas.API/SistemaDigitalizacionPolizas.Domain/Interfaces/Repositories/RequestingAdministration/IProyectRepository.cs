using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration
{
    public interface IProyectRepository
    {

        //Agregar un nuevo proyecto
        Task<Proyect?> AddAsync(Proyect unit);
        //Obtener un prpyecto por su codigo
        Task<Proyect?> GetByCodeAsync(int code);
        //Obtener todos los proyectos
        Task<IEnumerable<Proyect>> GetAllAsync();
        //Acctualizar la informacion de un proyecto
        Task<bool> UpdateAsync(Proyect unit);
        //Desactivar un proyecto
        Task<bool> DeleteAsync(int code, int userId);
    }
}