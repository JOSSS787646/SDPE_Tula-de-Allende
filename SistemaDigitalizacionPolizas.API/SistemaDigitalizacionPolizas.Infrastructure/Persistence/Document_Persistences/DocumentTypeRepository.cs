using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences
{
    public class DocumentTypeRepository : IDocumentTypeRepository
    {
        private readonly SdpeDbContext _context;

        public DocumentTypeRepository(SdpeDbContext context)
        {
            _context = context;
        }


        public async Task<DocumentType?> AddAsync(DocumentType entity)
        {
            var exists = await _context.Documents
                .AnyAsync(c => c.IdDocumentType == entity.IdDocumentType);

            if (exists)
                return null;

            await _context.Documents.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }





        public async Task<DocumentType?> GetByNameAsync(string documentName)
        {
            return await _context.Documents
                .FirstOrDefaultAsync(c => c.DocumentName == documentName);
        }



        public async Task<IEnumerable<DocumentType>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Documents
                .AsNoTracking()
                .OrderBy(d => d.IdDocumentType)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<bool> UpdateAsync(DocumentType unit)
        {
            _context.Documents.Update(unit);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DocumentType?> GetByIdAsync(int id)
        {
            return await _context.Documents
                .FirstOrDefaultAsync(x => x.IdDocumentType == id);
        }


        public async Task<bool> DeleteAsync(string documentName)
        {
            var document = await _context.Documents
                .FirstOrDefaultAsync(c => c.DocumentName == documentName);

            if (document == null)
                return false;

            document.Active = false;
            document.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }











    }
}
