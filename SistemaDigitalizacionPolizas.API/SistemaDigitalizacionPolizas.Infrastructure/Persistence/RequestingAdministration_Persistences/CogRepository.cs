using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.RequestingAdministration_Persistences
{
    public class CogRepository : ICogRepository
    {

        private readonly SdpeDbContext _context;

        public CogRepository(SdpeDbContext context)
        {
            _context = context;
        }


        //Obtiene todos los COG
        public async Task<IEnumerable<COG>> GetAllAsync()
        {
            return await _context.Cog
                .AsNoTracking()
                .ToListAsync();
        }

        //Obtiene un COG por su ID

        public async Task<COG?> GetByCodeAsync(int code)
        {
            return await _context.Cog
                .FirstOrDefaultAsync(c => c.Code == code);
        }



        //Agrega un COG 
        public async Task<COG?> AddAsync(COG unit)
        {
            var exists = await _context.Cog
                .AnyAsync(c => c.Code == unit.Code);

            if (exists)
                return null;

            unit.Active = true;
            unit.CreatedAt = DateTime.Now;

            await _context.Cog.AddAsync(unit);
            await _context.SaveChangesAsync();
            return unit;
        }


        //Actualiza un COG
        public async Task<bool> UpdateAsync(COG unit)
        {
            var existing = await _context.Cog
                .FirstOrDefaultAsync(c => c.Code == unit.Code);

            if (existing == null)
                return false;

            existing.Description = unit.Description;
            existing.Active = unit.Active;

            existing.UpdatedBy = unit.UpdatedBy; 
            existing.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }


        //Elimina un COG

        public async Task<bool> DeleteAsync(int code, int userId)
        {
            var cog = await _context.Cog
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
