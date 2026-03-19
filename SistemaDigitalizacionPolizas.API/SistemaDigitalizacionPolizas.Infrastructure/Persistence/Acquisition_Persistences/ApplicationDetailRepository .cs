using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Acquisition_Persistences
{
    public class ApplicationDetailRepository : IApplicationDetailRepository
    {
        private readonly SdpeDbContext _context;

        public ApplicationDetailRepository(SdpeDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApplicationDetail>> GetByRequestIdAsync(int requestId)
        {
            return await _context.ApplicationDetails
                .AsNoTracking()
                .Where(x => x.ApplicationId == requestId && x.Active)
                .ToListAsync();
        }

        public async Task<ApplicationDetail?> GetByIdAsync(int detailId)
        {
            return await _context.ApplicationDetails
                .FirstOrDefaultAsync(x => x.IdDetail == detailId && x.Active);
        }

        public async Task<int> AddAsync(ApplicationDetail detail)
        {
            await _context.ApplicationDetails.AddAsync(detail);
            await _context.SaveChangesAsync();

            return detail.IdDetail;
        }

        public async Task UpdateAsync(ApplicationDetail detail)
        {
            _context.ApplicationDetails.Update(detail);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int detailId)
        {
            var entity = await _context.ApplicationDetails
                .FirstOrDefaultAsync(x => x.IdDetail == detailId);

            if (entity == null)
                throw new Exception("Detail not found");

            _context.ApplicationDetails.Remove(entity);
            await _context.SaveChangesAsync();
        }


        public async Task UpsertRangeAsync(int requestId, List<ApplicationDetail> details)
        {
            var existing = await _context.ApplicationDetails
                .Where(x => x.ApplicationId == requestId)
                .ToListAsync();

            foreach (var item in details)
            {
                if (item.IdDetail == 0)
                {
                    // ➕ INSERT
                    item.ApplicationId = requestId;
                    await _context.ApplicationDetails.AddAsync(item);
                }
                else
                {
                    // 🔄 UPDATE
                    var existingEntity = existing
                        .FirstOrDefault(x => x.IdDetail == item.IdDetail);

                    if (existingEntity != null)
                    {
                        _context.Entry(existingEntity).CurrentValues.SetValues(item);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
