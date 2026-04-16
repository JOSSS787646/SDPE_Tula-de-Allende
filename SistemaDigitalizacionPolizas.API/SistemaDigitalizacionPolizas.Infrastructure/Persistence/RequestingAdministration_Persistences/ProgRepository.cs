using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.RequestingAdministration_Persistences
{
    public class ProgRepository : IProgRepository
    {
        private readonly SdpeDbContext _context;

        public ProgRepository(SdpeDbContext context)
        {
            _context = context;
        }


        // =========================
        // OBTENER TODOS
        // =========================
        public async Task<IEnumerable<Prog>> GetAllAsync()
        {
            return await _context.Progs
                .AsNoTracking()

                .ToListAsync();
        }

        // =========================
        // OBTENER POR CÓDIGO
        // =========================
        public async Task<Prog?> GetByCodeAsync(int code)
        {
            return await _context.Progs
                .FirstOrDefaultAsync(x => x.Code == code);
        }


        // ===============================
        // GET BY ID
        // ===============================

        public async Task<Prog?> GetByIdAsync(int id)
        {
            return await _context.Progs
                .FirstOrDefaultAsync(x => x.idProg == id);
        }


        // =========================
        // AGREGAR
        // =========================
        public async Task<Prog?> AddAsync(Prog unit)
        {
            var exists = await _context.Progs
                .AnyAsync(x => x.Code == unit.Code);

            if (exists)
                return null;

            unit.Active = true;
            unit.CreatedAt = DateTime.UtcNow;

            await _context.Progs.AddAsync(unit);
            await _context.SaveChangesAsync();

            return unit;
        }

        // =========================
        // ACTUALIZAR
        // =========================
        public async Task<bool> UpdateAsync(Prog unit)
        {
            _context.Progs.Update(unit);
            await _context.SaveChangesAsync();
            return true;
        }


        // =========================
        // ELIMINADO LÓGICO
        // =========================
        public async Task<bool> DeleteAsync(int code, int userId)
        {
            var proyect = await _context.Progs
                .FirstOrDefaultAsync(x => x.Code == code);

            if (proyect == null)
                return false;

            proyect.Active = false;
            proyect.UpdatedBy = userId;
            proyect.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
