using SistemaDigitalizacionPolizas.Domain.Dtos.Beneficiary;
using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Community
{
    public interface IBeneficiaryRepository
    {
        Task<Beneficiary> AddAsync(Beneficiary unit);

        Task<bool> ExistsByCurpAsync(string curp);
        Task<bool> ExistsByIneAsync(string ine);
        Task<List<BeneficiaryDto>> GetAllAsync();
        Task<bool> UpdateAsync(Beneficiary unit);
        Task<bool> DeleteAsync(string curp, int beneficiaryId);
        Task<Beneficiary?> GetByIdAsync(int id);
        Task<Beneficiary?> GetByCurpAsync(string curp);

    }
}
