using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences
{
    public class DocumentExpedientRepository : IDocumentExpedientRepository
    {
        private readonly SdpeDbContext _context;

        public DocumentExpedientRepository(SdpeDbContext context)
        {
            _context = context;
        }

        public async Task<ExpedientDocument> AddAsync(ExpedientDocument entity)
        {
            await _context.ExpedientDocuments.AddAsync(entity);
            return entity;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }




        public async Task<(List<ExpedientDocument> Items, int Total)>
     GetByClassificationAsync(int classificationId, int page, int pageSize)
        {
            var query = _context.ExpedientDocuments
                .AsNoTracking()
                .Include(x => x.DocumentType)
                .Include(x => x.DocumentStatus) // 🔥 CLAVE: cargar el estado del documento
                .Where(x =>
                    x.DocumentType.Classifications
                        .Any(c => c.ClassificationAcquisitionId == classificationId)
                );

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}
