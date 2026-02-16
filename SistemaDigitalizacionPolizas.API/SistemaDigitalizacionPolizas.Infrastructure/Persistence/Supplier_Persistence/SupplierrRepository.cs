using SistemaDigitalizacionPolizas.Domain.Entities.Supplier_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Supplier_Persistence
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly SdpeDbContext _context;

        public SupplierRepository(SdpeDbContext context)
        {
            _context = context;
        }

        // ===============================
        // GET ALL
        // ===============================
        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers
                .AsNoTracking()
                .ToListAsync();
        }

        // ===============================
        // GET BY ID
        // ===============================
        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers
                .FirstOrDefaultAsync(x => x.IdSupplier == id);
        }

        // ===============================
        // GET BY RFC
        // ===============================
        public async Task<Supplier?> GetByRfcAsync(string rfc)
        {
            var normalized = rfc.Trim().ToUpper();

            return await _context.Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Rfc == normalized);
        }

        // ===============================
        // EXISTS BY RFC
        // ===============================
        public async Task<bool> ExistsByRfcAsync(string rfc)
        {
            var normalized = rfc.Trim().ToUpper();

            return await _context.Suppliers
                .AsNoTracking()
                .AnyAsync(x => x.Rfc == normalized);
        }
        // ===============================
        // EXISTS BY PHONE
        // ===============================

        public async Task<bool> ExistsByPhoneAsync(string phone)
        {
            var normalized = phone.Trim();
            return await _context.Suppliers
                .AsNoTracking()
                .AnyAsync(x => x.Phone == normalized && x.Active);
        }


        // ===============================
        // ADD
        // ===============================
        public async Task<Supplier> AddAsync(Supplier unit)
        {
            unit.Rfc = unit.Rfc.Trim().ToUpper();
            unit.Active = true;
            unit.CreatedAt = DateTime.UtcNow;

            await _context.Suppliers.AddAsync(unit);
            await _context.SaveChangesAsync();

            return unit;
        }

        // ===============================
        // UPDATE
        // ===============================
        public async Task<bool> UpdateAsync(Supplier unit)
        {
            _context.Suppliers.Update(unit);
            await _context.SaveChangesAsync();
            return true;
        }

        // ===============================
        // DELETE (SOFT DELETE)
        // ===============================
        public async Task<bool> DeleteAsync(string rfc, int supplierId)
        {
            var normalized = rfc.Trim().ToUpper();

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(x => x.Rfc == normalized);

            if (supplier is null)
                return false;

            supplier.Active = false;
            supplier.UpdatedBy = supplierId;
            supplier.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }


        // ===============================
        // Trar¿e todos los proveedores paginados 
        // ===============================

        public async Task<List<Supplier>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.Suppliers
                .AsNoTracking()
                .OrderBy(x => x.IdSupplier)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
