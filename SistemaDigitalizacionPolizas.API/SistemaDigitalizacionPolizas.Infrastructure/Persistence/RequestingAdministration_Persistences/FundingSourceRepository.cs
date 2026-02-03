using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;

public class FundingSourceRepository : IFundingSourceRepository
{
    private readonly SdpeDbContext _context;

    public FundingSourceRepository(SdpeDbContext context)
    {
        _context = context;
    }

    //Obtiene todos los Fondos de financiamiento
    public async Task<IEnumerable<FundingSource>> GetAllAsync()
    {
        return await _context.FundingSources
            .AsNoTracking()
            .ToListAsync();
    }

    //Obtiene un un fondo de financiamiento por su codigo

    public async Task<FundingSource?> GetByCodeAsync(int code)
    {
        return await _context.FundingSources
            .FirstOrDefaultAsync(c => c.Code == code);
    }



    //Agrega un  fondo de financiamiento nuevo 
    public async Task<FundingSource?> AddAsync(FundingSource unit)
    {
        var exists = await _context.FundingSources
            .AnyAsync(c => c.Code == unit.Code);

        if (exists)
            return null;

        await _context.FundingSources.AddAsync(unit);
        await _context.SaveChangesAsync();
        return unit;
    }


    //Actualiza un  fondo de financiamiento
    public async Task<bool> UpdateAsync(FundingSource unit)
    {
        var existing = await _context.FundingSources
            .FirstOrDefaultAsync(c => c.Code == unit.Code);

        if (existing == null)
            return false;

        existing.Description = unit.Description;
        existing.Active = unit.Active;

        await _context.SaveChangesAsync();
        return true;
    }


    //Elimina un  fondo de financiamiento

    public async Task<bool> DeleteAsync(int code)
    {
        var fundingSource = await _context.FundingSources
            .FirstOrDefaultAsync(c => c.Code == code);
        if (fundingSource == null)
            return false;
        _context.FundingSources.Remove(fundingSource);
        await _context.SaveChangesAsync();
        return true;
    }

}
