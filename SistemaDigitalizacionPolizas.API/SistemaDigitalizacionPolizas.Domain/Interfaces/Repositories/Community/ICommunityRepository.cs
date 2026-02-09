using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Communitys
{
    public interface ICommunityRepository
    {

        Task<Community?> AddAsync(Community unit);
        Task<Community?> GetByCodeAsync(int code);
        Task<IEnumerable<Community>> GetAllAsync();
        Task<bool> UpdateAsync(Community unit);
        Task<bool> DeleteAsync(int code, int idCommunity);
        Task<Community?> GetByIdAsync(int idCommunity);

    }
}
