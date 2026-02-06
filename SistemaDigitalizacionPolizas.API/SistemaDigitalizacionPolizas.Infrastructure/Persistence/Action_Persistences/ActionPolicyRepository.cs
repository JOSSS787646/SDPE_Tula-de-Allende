using SistemaDigitalizacionPolizas.Domain.Entities.Actions_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Action_Persistences
{
    public class ActionPolicyRepository : IActionPolicyRepository
    {
        private readonly SdpeDbContext _context;

        public ActionPolicyRepository(SdpeDbContext context)
        {
            _context = context;
        }

        //Obtiene todos las Accciones
        public async Task<IEnumerable<ActionsPolicy>> GetAllAsync()
        {
            return await _context.Actions
                .AsNoTracking()
                .ToListAsync();
        }

        //Obtiene una accion por su ID

        public async Task<ActionsPolicy?> GetByCodeAsync(int code)
        {
            return await _context.Actions
                .FirstOrDefaultAsync(c => c.Code == code);
        }



        //Agrega una accion
        public async Task<ActionsPolicy?> AddAsync(ActionsPolicy unit)
        {
            var exists = await _context.Actions
                .AnyAsync(c => c.Code == unit.Code);

            if (exists)
                return null;

            unit.Active = true;
            unit.CreatedAt = DateTime.Now;

            await _context.Actions.AddAsync(unit);
            await _context.SaveChangesAsync();
            return unit;
        }


        //Actualiza una accion
        public async Task<bool> UpdateAsync(ActionsPolicy unit)
        {
            var existing = await _context.Actions
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


        //Elimina una accion

        public async Task<bool> DeleteAsync(int code, int userId)
        {
            var cog = await _context.Actions
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
