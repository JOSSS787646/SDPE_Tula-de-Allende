using SistemaDigitalizacionPolizas.Domain.Entities.Areas_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.AdministrtiveUnit
{
    public interface IAdministrativeUnit
    {
        Task<AdministrativeUnit?> CreateAsync(AdministrativeUnit unit);
        Task<AdministrativeUnit?> GetByIdAsync(int id);
        Task<IEnumerable<AdministrativeUnit>> GetAllAsync();
        Task<bool> UpdateAsync(AdministrativeUnit unit);
        Task<bool> DeleteAsync(int id);
    }
}
