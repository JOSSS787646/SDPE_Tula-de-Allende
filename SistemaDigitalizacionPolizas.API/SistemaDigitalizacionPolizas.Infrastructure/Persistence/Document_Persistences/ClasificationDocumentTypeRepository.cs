using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences
{
    public class ClasificationDocumentTypeRepository : IClasificationDocumentTypeRepository
    {
        private readonly SdpeDbContext _context;

        public ClasificationDocumentTypeRepository(SdpeDbContext context)
        {
            _context = context;
        }

        // =========================================================
        // 🔹 CARGA MASIVA (REEMPLAZAR COMPLETO)
        // =========================================================
        public async Task ReplaceAsync(
            int acquisitionClassificationId,
            IEnumerable<ClasificationDocumentType> entities)
        {
            // 1️⃣ Obtener relaciones actuales
            var existing = await _context.ClasificationDocumentTypes
                .Where(x => x.ClassificationAcquisitionId
                            == acquisitionClassificationId)
                .ToListAsync();

            // 2️⃣ Eliminar actuales
            if (existing.Any())
                _context.ClasificationDocumentTypes.RemoveRange(existing);

            // 3️⃣ Insertar nuevas
            await _context.ClasificationDocumentTypes
                .AddRangeAsync(entities);

            // 4️⃣ Guardar cambios
            await _context.SaveChangesAsync();
        }

        // =========================================================
        // 🔹 OBTENER DOCUMENTOS POR CLASIFICACIÓN
        // =========================================================
        public async Task<List<ClasificationDocumentType>>
            GetByClassificationAsync(int acquisitionClassificationId)
        {
            return await _context.ClasificationDocumentTypes
                .AsNoTracking()
                .Where(x => x.ClassificationAcquisitionId
                            == acquisitionClassificationId)
                .Include(x => x.DocumentType) // opcional si necesitas datos del documento
                .ToListAsync();
        }

        // =========================================================
        // 🔹 ELIMINAR TODAS LAS RELACIONES DE UNA CLASIFICACIÓN
        // =========================================================
        public async Task RemoveByClassificationAsync(
            int acquisitionClassificationId)
        {
            var existing = await _context.ClasificationDocumentTypes
                .Where(x => x.ClassificationAcquisitionId
                            == acquisitionClassificationId)
                .ToListAsync();

            if (existing.Any())
            {
                _context.ClasificationDocumentTypes.RemoveRange(existing);
                await _context.SaveChangesAsync();
            }
        }

        // =========================================================
        // 🔹 VERIFICAR SI EXISTE UNA RELACIÓN
        // =========================================================
        public async Task<bool> ExistsAsync(
            int acquisitionClassificationId,
            int documentTypeId)
        {
            return await _context.ClasificationDocumentTypes
                .AnyAsync(x =>
                    x.ClassificationAcquisitionId
                        == acquisitionClassificationId &&
                    x.DocumentTypeId
                        == documentTypeId);
        }


    }
}
