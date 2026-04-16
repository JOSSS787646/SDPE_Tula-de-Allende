using SistemaDigitalizacionPolizas.Domain.Entities.ApplicationStatus;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.ApplicationStatus_Persistences
{
    public class DocumentStatusRepository: IDocumentStatusRepository
    {

        private readonly SdpeDbContext _context;

        public DocumentStatusRepository(SdpeDbContext context)
        {
            _context = context;
        }

        //Obtiene todos los COG
        public async Task<IEnumerable<DocumentStatus>> GetAllAsync()
        {
            return await _context.DocumentStatuses
                .AsNoTracking()
                .ToListAsync();
        }

        //Obtiene un COG por su ID

        public async Task<DocumentStatus?> GetByCodeAsync(int code)
        {
            return await _context.DocumentStatuses
                .FirstOrDefaultAsync(c => c.Code == code);
        }


        // ===============================
        // GET BY ID
        // ===============================

        public async Task<DocumentStatus?> GetByIdAsync(int id)
        {
            return await _context.DocumentStatuses
                .FirstOrDefaultAsync(x => x.idDocumentStatus == id);
        }



        //Agrega un COG 
        public async Task<DocumentStatus?> AddAsync(DocumentStatus unit)
        {
            var exists = await _context.DocumentStatuses
                .AnyAsync(c => c.Code == unit.Code);

            if (exists)
                return null;

            unit.Active = true;
            unit.CreatedAt = DateTime.UtcNow;

            await _context.DocumentStatuses.AddAsync(unit);
            await _context.SaveChangesAsync();
            return unit;
        }


        //Actualiza un COG
        public async Task<bool> UpdateAsync(DocumentStatus unit)
        {
            _context.DocumentStatuses.Update(unit);
            await _context.SaveChangesAsync();
            return true;
        }


        //Elimina un COG

        public async Task<bool> DeleteAsync(int code, int userId)
        {
            var cog = await _context.DocumentStatuses
                .FirstOrDefaultAsync(x => x.Code == code);

            if (cog == null)
                return false;

            cog.Active = false;
            cog.UpdatedBy = userId;
            cog.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }


    }
}
