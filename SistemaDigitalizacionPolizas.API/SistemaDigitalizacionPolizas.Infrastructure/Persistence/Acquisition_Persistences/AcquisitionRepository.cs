using SistemaDigitalizacionPolizas.Domain.Entities.Acquisition_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Acquisition_Persistences
{
    public class AcquisitionRepository : IAcquisitionRepository
    {


        /// <summary>
        /// ///no sivr etes en lo asboulduooooo eliminar
        /// </summary>
        private readonly SdpeDbContext _context;

        public AcquisitionRepository(SdpeDbContext context)
        {
            _context = context;
        }


        //Obtiene todos los COG
        public async Task<IEnumerable<AcquisitionType>> GetAllAsync()
        {
            return await _context.AcquisitionTypes
                .AsNoTracking()
                .ToListAsync();
        }

        //Obtiene un COG por su ID

        public async Task<AcquisitionType?> GetByCodeAsync(int code)
        {
            return await _context.AcquisitionTypes
                .FirstOrDefaultAsync(c => c.Code == code);
        }


        // ===============================
        // GET BY ID
        // ===============================

        public async Task<AcquisitionType?> GetByIdAsync(int idAcquisitionType)
        {
            return await _context.AcquisitionTypes
                .FirstOrDefaultAsync(x => x.idAcquisitionType == idAcquisitionType);
        }



        //Agrega un COG 
        public async Task<AcquisitionType?> AddAsync(AcquisitionType unit)
        {
            var exists = await _context.AcquisitionTypes
                .AnyAsync(c => c.Code == unit.Code);

            if (exists)
                return null;

            unit.Active = true;
            unit.CreatedAt = DateTime.Now;

            await _context.AcquisitionTypes.AddAsync(unit);
            await _context.SaveChangesAsync();
            return unit;
        }


        //Actualiza un COG
        public async Task<bool> UpdateAsync(AcquisitionType unit)
        {
            _context.AcquisitionTypes.Update(unit);
            await _context.SaveChangesAsync();
            return true;
        }



        //Elimina un COG

        public async Task<bool> DeleteAsync(int code, int UserId)
        {
            var cog = await _context.AcquisitionTypes
                .FirstOrDefaultAsync(x => x.Code == code);

            if (cog == null)
                return false;

            cog.Active = false;
            cog.UpdatedBy = UserId;
            cog.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
