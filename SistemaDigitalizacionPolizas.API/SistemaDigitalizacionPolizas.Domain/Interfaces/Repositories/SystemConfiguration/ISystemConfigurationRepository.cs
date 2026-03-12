using SistemaDigitalizacionPolizas.Domain.Entities.SystemConfiguration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.ISystemConfiguration
{
    public interface ISystemConfigurationRepository
    {
        Task<SystemConfiguration?> GetActiveAsync();

        Task AddAsync(SystemConfiguration configuration);

        Task UpdateAsync(SystemConfiguration configuration);
    }
}
