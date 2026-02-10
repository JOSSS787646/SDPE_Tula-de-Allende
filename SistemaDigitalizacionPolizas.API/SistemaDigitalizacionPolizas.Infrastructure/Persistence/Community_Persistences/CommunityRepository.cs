using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Community_Persistences
{
    public class CommunityRepository: ICommunityRepository
    {

        private readonly SdpeDbContext _context;

        public CommunityRepository(SdpeDbContext sdpeDbContext)
        {
            _context = sdpeDbContext;
        }

        //Obtiene todos los COG
        public async Task<IEnumerable<Community>> GetAllAsync()
        {
            return await _context.Communities
                .AsNoTracking()
                .ToListAsync();
        }

        //Obtiene un COG por su ID

        public async Task<Community?> GetByCodeAsync(int code)
        {
            return await _context.Communities
                .FirstOrDefaultAsync(c => c.Code == code);
        }


        // ===============================
        // GET BY ID
        // ===============================

        public async Task<Community?> GetByIdAsync(int id)
        {
            return await _context.Communities
                .FirstOrDefaultAsync(x => x.idCommunity == id);
        }



        //Agrega un COG 
        public async Task<Community?> AddAsync(Community unit)
        {
            var exists = await _context.Communities
                .AnyAsync(c => c.Code == unit.Code);

            if (exists)
                return null;

            unit.Active = true;
            unit.CreatedAt = DateTime.Now;

            await _context.Communities.AddAsync(unit);
            await _context.SaveChangesAsync();
            return unit;
        }


        //Actualiza un COG
        public async Task<bool> UpdateAsync(Community unit)
        {
            _context.Communities.Update(unit);
            await _context.SaveChangesAsync();
            return true;
        }


        //Elimina un COG

        public async Task<bool> DeleteAsync(int code, int idcommunity)
        {
            var cog = await _context.Communities
                .FirstOrDefaultAsync(x => x.Code == code);

            if (cog == null)
                return false;

            cog.Active = false;
            cog.UpdatedBy = idcommunity;
            cog.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }


    }
}
