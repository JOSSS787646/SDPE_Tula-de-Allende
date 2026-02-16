using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Community_Persistences
{
    public class BeneficiaryRepository: IBeneficiaryRepository
    {
        private readonly SdpeDbContext _context;

        public BeneficiaryRepository(SdpeDbContext context)
        {
            _context = context;
        }

        //Obtiene todos los COG
        public async Task<IEnumerable<Beneficiary>> GetAllAsync()
        {
            return await _context.Beneficiaries
                .AsNoTracking()
                .ToListAsync();
        }

        // Valida si ya existe un beneficiario con la CURP
        public async Task<bool> ExistsByCurpAsync(string curp)
        {
            return await _context.Beneficiaries
                .AsNoTracking()
                .AnyAsync(b => b.Curp == curp);
        }

        // Valida si ya existe un beneficiario con el INE
        public async Task<bool> ExistsByIneAsync(string ine)
        {
            return await _context.Beneficiaries
                .AsNoTracking()
                .AnyAsync(b => b.Ine == ine);
        }

        // ===============================
        // GET BY CURP
        // ===============================
        public async Task<Beneficiary?> GetByCurpAsync(string curp)
        {
            return await _context.Beneficiaries
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Curp == curp);
        }


        // ===============================
        // GET BY ID
        // ===============================

        public async Task<Beneficiary?> GetByIdAsync(int id)
        {
            return await _context.Beneficiaries
                .FirstOrDefaultAsync(x => x.IdBeneficiary == id);
        }



        //Agrega un COG 
        public async Task<Beneficiary> AddAsync(Beneficiary unit)
        {
            unit.Active = true;
            unit.CreatedAt = DateTime.UtcNow;

            await _context.Beneficiaries.AddAsync(unit);
            await _context.SaveChangesAsync();

            return unit;
        }


        //Actualiza un COG
        public async Task<bool> UpdateAsync(Beneficiary unit)
        {
            _context.Beneficiaries.Update(unit);
            await _context.SaveChangesAsync();
            return true;
        }


        //Elimina un COG

        public async Task<bool> DeleteAsync(string curp, int idBeneficiary)
        {
            var beneficiary = await _context.Beneficiaries
                .FirstOrDefaultAsync(x => x.Curp == curp);

            if (beneficiary == null)
                return false;

            beneficiary.Active = false;
            beneficiary.ModifiedBy = idBeneficiary;
            beneficiary.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
