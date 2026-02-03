using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.AdministrtiveUnit;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.AdministrativeUnit_Persistences
{
    public class AdministrativeUnitRepository : IAdministrativeUnit
    {
        private readonly SdpeDbContext _context;

        public AdministrativeUnitRepository(SdpeDbContext context)
        {
            _context = context;
        }

        // ===============================
        // CREATE
        // ===============================
        public async Task<AdministrativeUnit?> CreateAsync(AdministrativeUnit unit)
        {
            _context.AdministrativeUnits.Add(unit);
            await _context.SaveChangesAsync();
            return unit;
        }

        // ===============================
        // GET BY CODE
        // ===============================
        public async Task<AdministrativeUnit?> GetByCodeAsync(int code)
        {
            return await _context.AdministrativeUnits
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        // ===============================
        // GET ALL
        // ===============================
        public async Task<IEnumerable<AdministrativeUnit>> GetAllAsync()
        {
            return await _context.AdministrativeUnits
                .OrderBy(x => x.Description)
                .ToListAsync();
        }

        // ===============================
        // UPDATE
        // ===============================
        public async Task<bool> UpdateAsync(AdministrativeUnit unit)
        {
            var exists = await _context.AdministrativeUnits
                .AnyAsync(x => x.Code == unit.Code);

            if (!exists)
                return false;

            _context.AdministrativeUnits.Update(unit);
            await _context.SaveChangesAsync();
            return true;
        }

        // ===============================
        // DELETE
        // ===============================
        public async Task<bool> DeleteAsync(int code)
        {
            var entity = await _context.AdministrativeUnits
                .FirstOrDefaultAsync(x => x.Code == code);

            if (entity == null)
                return false;

            _context.AdministrativeUnits.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
