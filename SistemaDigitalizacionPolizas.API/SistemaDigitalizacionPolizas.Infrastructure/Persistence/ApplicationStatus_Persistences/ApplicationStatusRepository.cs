using SistemaDigitalizacionPolizas.Domain.Entities.ApplicationStatus;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;

using SSistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.ApplicationStatus_Persistences
{
    public class ApplicationStatusRepository: IApplicationStatusRepository

    {

        private readonly SdpeDbContext _context;

        public ApplicationStatusRepository(SdpeDbContext context)
        {
            _context = context;
        }

        //Obtiene todos los COG
        public async Task<IEnumerable<ApplicationStatus>> GetAllAsync()
        {
            return await _context.ApplicationStatuses
                .AsNoTracking()
                .ToListAsync();
        }

        //Obtiene un COG por su ID

        public async Task<ApplicationStatus?> GetByCodeAsync(int code)
        {
            return await _context.ApplicationStatuses
                .FirstOrDefaultAsync(c => c.Code == code);
        }


        // ===============================
        // GET BY ID
        // ===============================

        public async Task<ApplicationStatus?> GetByIdAsync(int id)
        {
            return await _context.ApplicationStatuses
                .FirstOrDefaultAsync(x => x.IdApplicationStatus == id);
        }


    
        //Agrega un COG 
        public async Task<ApplicationStatus?> AddAsync(ApplicationStatus unit)
        {
            var exists = await _context.ApplicationStatuses
                .AnyAsync(c => c.Code == unit.Code);

            if (exists)
                return null;

            unit.Active = true;
            unit.CreatedAt = DateTime.Now;

            await _context.ApplicationStatuses.AddAsync(unit);
            await _context.SaveChangesAsync();
            return unit;
        }


        //Actualiza un COG
        public async Task<bool> UpdateAsync(ApplicationStatus unit)
        {
            _context.ApplicationStatuses.Update(unit);
            await _context.SaveChangesAsync();
            return true;
        }


        //Elimina un COG

        public async Task<bool> DeleteAsync(int code, int userId)
        {
            var cog = await _context.ApplicationStatuses
                .FirstOrDefaultAsync(x => x.Code == code);

            if (cog == null)
                return false;

            cog.Active = false;
            cog.UpdatedBy = userId;
            cog.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
