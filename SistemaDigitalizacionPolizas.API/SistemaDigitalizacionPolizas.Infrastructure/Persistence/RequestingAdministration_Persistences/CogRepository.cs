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


        // ===============================
        // GET BY ID
        // ===============================

        public async Task<COG?> GetByIdAsync(int id)
        {
            return await _context.Cog
                .FirstOrDefaultAsync(x => x.idCog == id);
        }



        //Agrega un COG 
        public async Task<COG?> AddAsync(COG unit)
        {
            var exists = await _context.Cog
                .AnyAsync(c => c.Code == unit.Code);

            if (exists)
                return null;

            unit.Active = true;
            unit.CreatedAt = DateTime.UtcNow;

            await _context.Cog.AddAsync(unit);
            await _context.SaveChangesAsync();
            return unit;
        }


        //Actualiza un COG
        public async Task<bool> UpdateAsync(COG unit)
        {
            _context.Cog.Update(unit);
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
            cog.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }



    }
}
