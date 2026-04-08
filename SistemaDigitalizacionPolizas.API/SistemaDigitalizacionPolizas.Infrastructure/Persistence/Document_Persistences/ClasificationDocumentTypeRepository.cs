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
        public async Task UpsertRangeAsync(
     int acquisitionClassificationId,
     IEnumerable<ClasificationDocumentType> entities)
        {
            // 1️⃣ Traer existentes de BD
            var existing = await _context.ClasificationDocumentTypes
                .Where(x => x.ClassificationAcquisitionId == acquisitionClassificationId)
                .ToListAsync();

            // 2️⃣ Convertir a diccionario para búsqueda rápida
            var existingDict = existing.ToDictionary(
                x => x.DocumentTypeId,
                x => x
            );

            var incomingDict = entities.ToDictionary(
                x => x.DocumentTypeId,
                x => x
            );

            // 3️⃣ INSERT o UPDATE
            foreach (var entity in entities)
            {
                if (existingDict.TryGetValue(entity.DocumentTypeId, out var existingEntity))
                {
                    // 🔄 UPDATE
                    existingEntity.IsRequired = entity.IsRequired;
                    existingEntity.Active = entity.Active;

                    // opcional: marcar como modificado
                    _context.ClasificationDocumentTypes.Update(existingEntity);
                }
                else
                {
                    // ➕ INSERT
                    await _context.ClasificationDocumentTypes.AddAsync(entity);
                }
            }

            // 4️⃣ DELETE (los que ya no vienen)
            var toDelete = existing
                .Where(x => !incomingDict.ContainsKey(x.DocumentTypeId))
                .ToList();

            if (toDelete.Any())
                _context.ClasificationDocumentTypes.RemoveRange(toDelete);

            // 5️⃣ Guardar cambios
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


        public async Task<List<ClasificationDocumentType>> GetRequiredByClassification(int classificationId)
        {
            return await _context.ClasificationDocumentTypes
                .Where(x =>
                    x.ClassificationAcquisitionId == classificationId &&
                    x.IsRequired &&
                    x.Active)
                .ToListAsync();
        }


    }
}
