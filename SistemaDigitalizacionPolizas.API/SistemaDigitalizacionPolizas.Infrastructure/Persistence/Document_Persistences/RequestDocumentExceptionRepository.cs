using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences
{
    public class RequestDocumentExceptionRepository : IRequestDocumentExceptionRepository
    {
        private readonly SdpeDbContext _context;

        public RequestDocumentExceptionRepository(SdpeDbContext context)
        {
            _context = context;
        }

        public async Task<RequestDocumentException?> GetByIdAsync(int id)
        {
            return await _context.RequestDocumentExceptions
                .Include(x => x.DocumentType)
                .Include(x => x.Request)
                .FirstOrDefaultAsync(x => x.IdRequestDocumentException == id && x.Active);
        }

        public async Task<IEnumerable<RequestDocumentException>> GetByRequestIdAsync(int requestId)
        {
            return await _context.RequestDocumentExceptions
                .Where(x => x.IdRequest == requestId && x.Active)
                .Include(x => x.DocumentType)
                .ToListAsync();
        }

        public async Task AddAsync(RequestDocumentException entity)
        {
            await _context.RequestDocumentExceptions.AddAsync(entity);
        }

        public async Task UpdateAsync(RequestDocumentException entity)
        {
            _context.RequestDocumentExceptions.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.RequestDocumentExceptions
                .FirstOrDefaultAsync(x => x.IdRequestDocumentException == id);

            if (entity != null)
            {
                entity.Active = false; // Soft delete
                _context.RequestDocumentExceptions.Update(entity);
            }
        }

        public async Task<bool> ExistsAsync(int requestId, int documentTypeId)
        {
            return await _context.RequestDocumentExceptions
                .AnyAsync(x => x.IdRequest == requestId
                            && x.IdDocumentType == documentTypeId
                            && x.Active);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
