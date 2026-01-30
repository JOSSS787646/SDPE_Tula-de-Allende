using SistemaDigitalizacionPolizas.Domain.Entities.Areas_Entities;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.AdministrtiveUnit
{
    public interface IAdministrativeUnit
    {
        Task<AdministrativeUnit?> CreateAsync(AdministrativeUnit unit);
        Task<AdministrativeUnit?> GetByCodeAsync(int code);
        Task<IEnumerable<AdministrativeUnit>> GetAllAsync();
        Task<bool> UpdateAsync(AdministrativeUnit unit);
        Task<bool> DeleteAsync(int code);
    }
}
