using SistemaDigitalizacionPolizas.Domain.Entities.Areas_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration
{
    public interface ICogRepository
    {
        Task<COG?> AddAsync(COG unit);
        Task<COG?> GetByCodeAsync(int code);
        Task<IEnumerable<COG>> GetAllAsync();
        Task<bool> UpdateAsync(COG unit);
        Task<bool> DeleteAsync(int code, int userId);
        Task<COG?> GetByIdAsync(int id);
    }
}
