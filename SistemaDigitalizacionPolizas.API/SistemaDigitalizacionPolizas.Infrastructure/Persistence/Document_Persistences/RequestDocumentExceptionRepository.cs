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


        public async Task AddAsync(RequestDocumentException entity)
        {
            await _context.RequestDocumentExceptions.AddAsync(entity);
        }

        public async Task UpdateAsync(RequestDocumentException entity)
        {
            _context.RequestDocumentExceptions.Update(entity);
            await Task.CompletedTask;
        }

        public async Task<RequestDocumentException?>
    GetByRequestAndDocumentTypeAsync(int requestId, int documentTypeId)
        {
            return await _context.RequestDocumentExceptions
                .FirstOrDefaultAsync(x =>
                    x.IdRequest == requestId &&
                    x.IdDocumentType == documentTypeId);
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
