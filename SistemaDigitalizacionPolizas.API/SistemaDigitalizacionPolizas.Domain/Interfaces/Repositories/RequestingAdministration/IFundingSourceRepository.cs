using SistemaDigitalizacionPolizas.Domain.Entities.Areas_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration
{
    public interface IFundingSourceRepository
    {
        //Agregar un nuevo fondo
        Task<FundingSource?> AddAsync(FundingSource unit);
        //Obtener un fondo por su codigo
        Task<FundingSource?> GetByCodeAsync(int code);
        //Obtener todos los fondos
        Task<IEnumerable<FundingSource>> GetAllAsync();
        //Acctualizar la informacion de un fondo
        Task<bool> UpdateAsync(FundingSource unit);
        //Desactivar un fondo
        Task<bool> DeleteAsync(int code);
        Task<FundingSource?> GetByIdAsync(int id);
    }
}
