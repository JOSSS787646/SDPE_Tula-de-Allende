using SistemaDigitalizacionPolizas.Domain.Entities.SystemConfiguration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.ISystemConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.SystemConfiguration_Persistences
{
    public class SystemConfigurationRepository: ISystemConfigurationRepository
    {
        private readonly SdpeDbContext _context;

        public SystemConfigurationRepository(SdpeDbContext context)
        {
            _context = context;
        }


        public async Task<SystemConfiguration?> GetActiveAsync()
        {
            return await _context.SystemConfigurations
                .Where(x => x.Active)
                .OrderByDescending(x => x.IdConfiguration)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(SystemConfiguration configuration)
        {
            _context.SystemConfigurations.Add(configuration);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SystemConfiguration configuration)
        {
            _context.SystemConfigurations.Update(configuration);
            await _context.SaveChangesAsync();
        }
    }
}
