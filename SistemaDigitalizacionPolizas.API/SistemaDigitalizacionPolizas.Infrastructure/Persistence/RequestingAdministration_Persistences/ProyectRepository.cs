using Microsoft.EntityFrameworkCore;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.RequestingAdministration_Persistences
{
    public class ProyectRepository : IProyectRepository
    {
        private readonly SdpeDbContext _context;

        public ProyectRepository(SdpeDbContext context)
        {
            _context = context;
        }

        // =========================
        // OBTENER TODOS
        // =========================
        public async Task<IEnumerable<Proyect>> GetAllAsync()
        {
            return await _context.Proyects
                .AsNoTracking()
             
                .ToListAsync();
        }

        // =========================
        // OBTENER POR CÓDIGO
        // =========================
        public async Task<Proyect?> GetByCodeAsync(int code)
        {
            return await _context.Proyects
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        // =========================
        // AGREGAR
        // =========================
        public async Task<Proyect?> AddAsync(Proyect unit)
        {
            var exists = await _context.Proyects
                .AnyAsync(x => x.Code == unit.Code);

            if (exists)
                return null;

            // Auditoría inicial
            unit.Active = true;
            unit.CreatedAt = DateTime.Now;
            // unit.CreatedBy debe venir desde el usuario autenticado

            await _context.Proyects.AddAsync(unit);
            await _context.SaveChangesAsync();

            return unit;
        }


        // =========================
        // ACTUALIZAR
        // =========================
        public async Task<bool> UpdateAsync(Proyect unit)
        {
            _context.Proyects.Update(unit);
            await _context.SaveChangesAsync();
            return true;
        }


        // =========================
        // ELIMINADO LÓGICO
        // =========================
        public async Task<bool> DeleteAsync(int code, int userId)
        {
            var proyect = await _context.Proyects
                .FirstOrDefaultAsync(x => x.Code == code);

            if (proyect == null)
                return false;

            proyect.Active = false;
            proyect.UpdatedBy = userId;
            proyect.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        // ===============================
        // GET BY ID
        // ===============================

        public async Task<Proyect?> GetByIdAsync(int id)
        {
            return await _context.Proyects
                .FirstOrDefaultAsync(x => x.idProyect == id);
        }




        public async Task<Proyect?> GetByCodeIncludingInactiveAsync(int code)
        {
            return await _context.Proyects
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Code == code);
        }

    }
}
