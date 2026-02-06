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

            unit.Active = true;
            unit.CreatedAt = DateTime.Now;

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
            var entity = await _context.AdministrativeUnits
                .FirstOrDefaultAsync(x => x.Code == unit.Code);

            if (entity is null)
                return false;

            entity.Code = unit.Code;
            entity.Description = unit.Description;   
            entity.UpdatedBy = unit.UpdatedBy;
            entity.UpdatedAt = DateTime.Now;
            entity.Active = unit.Active;

            await _context.SaveChangesAsync();
            return true;
        }


        // ===============================
        // DELETE
        // ===============================
        public async Task<bool> DeleteAsync(int code, int userId)
        {
            var entity = await _context.AdministrativeUnits
                .FirstOrDefaultAsync(x => x.Code == code);

            if (entity == null)
                return false;

            entity.Active = false;
            entity.UpdatedBy = userId;
            entity.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
