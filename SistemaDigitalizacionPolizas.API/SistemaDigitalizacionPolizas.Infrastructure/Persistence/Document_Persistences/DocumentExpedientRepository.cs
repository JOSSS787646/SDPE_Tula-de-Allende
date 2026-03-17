using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument;
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

        public async Task AddRangeAsync(List<ExpedientDocument> entities)
        {
            await _context.ExpedientDocuments.AddRangeAsync(entities);
        }


        public async Task UpdateAsync(ExpedientDocument entity)
        {
            _context.ExpedientDocuments.Update(entity);
        }

        public async Task<ExpedientDocument?> GetByIdAsync(int id)
        {
            return await _context.ExpedientDocuments
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(ExpedientDocument entity)
        {
            _context.ExpedientDocuments.Update(entity);
        }

    

        public async Task<List<ExpedientDocument>> GetActiveByRequestId(int requestId)
        {
            return await _context.ExpedientDocuments
                .Where(x =>
                    x.RequestId == requestId &&
                    x.Active == true)
                .ToListAsync();
        }

        public void Delete(ExpedientDocument entity)
        {
            _context.ExpedientDocuments.Remove(entity);
        }

        public async Task<List<ExpedientDocumentSearchDto>>
    SearchByNameAsync(int requestId, string fileName)
        {
            return await _context.ExpedientDocuments
                .AsNoTracking()
                .Include(x => x.DocumentStatus)
                .Where(x =>
                    x.RequestId == requestId &&
                    x.Active == true &&
                    EF.Functions.Like(x.FileName, $"%{fileName}%")
                )
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ExpedientDocumentSearchDto
                {
                    Id = x.Id,
                    FileName = x.FileName,
                    FilePath = x.FilePath,
                    UploadDate = x.UploadDate,
                    Observations = x.Observations,
                    IdDocumentStatus = x.IdDocumentStatus,
                    DocumentStatusName = x.DocumentStatus.Description
                })
                .ToListAsync();
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


        public async Task<List<RequestDocumentChecklistDto>> GetChecklistByRequestAsync(int requestId)
        {
            var classificationId = await _context.AcquisitionRequests
                .Where(x => x.IdRequest == requestId)
                .Select(x => x.IdAcquisitionClassification)
                .FirstOrDefaultAsync();

            if (classificationId == null)
                return new List<RequestDocumentChecklistDto>();

            // 1️⃣ Traer documentos del expediente UNA vez
            var expedientDocs = await _context.ExpedientDocuments
                .Where(x => x.RequestId == requestId && x.Active == true)
                .AsNoTracking()
                .ToListAsync();

            // 2️⃣ Traer excepciones UNA vez
            var exceptions = await _context.RequestDocumentExceptions
                .Where(x => x.IdRequest == requestId)
                .AsNoTracking()
                .ToListAsync();

            // 3️⃣ Reglas de documentos por clasificación
            var documentRules = await (
                from tcd in _context.ClasificationDocumentTypes
                join td in _context.Documents
                    on tcd.DocumentTypeId equals td.IdDocumentType
                where tcd.ClassificationAcquisitionId == classificationId
                   && tcd.Active == true
                   && td.Active == true
                select new
                {
                    td.IdDocumentType,
                    td.DocumentName,
                    tcd.IsRequired
                }
            )
            .AsNoTracking()
               .ToListAsync();

            // 4️⃣ Construir DTO respetando tu modelo
            var result = documentRules.Select(rule =>
            {
                var docs = expedientDocs
                    .Where(x => x.DocumentTypeId == rule.IdDocumentType)
                    .ToList();

                return new RequestDocumentChecklistDto
                {
                    DocumentTypeId = rule.IdDocumentType,
                    DocumentName = rule.DocumentName,
                    RequiredByRule = rule.IsRequired,

                    NoApplies = exceptions.Any(x =>
                        x.IdDocumentType == rule.IdDocumentType &&
                        x.DoesNotApply == true),

                    Uploaded = docs.Any(),

                    Observations = docs
                        .Select(x => x.Observations)
                        .FirstOrDefault(),

                    FileIds = docs
                        .Select(x => x.Id)
                        .ToList(),

                    FileNames = docs
                        .Select(x => x.FileName)
                        .ToList(),

                    FileUrls = docs
                        .Select(x => x.FilePath)
                        .ToList(),

                    PreviewUrls = null
                };
            }).ToList();

            return result;
        }
        public async Task<IEnumerable<ExpedientDocumentPreviewDto>> GetDocumentsByClassificationAsync(int classificationId)
        {
            return await (
                from tcd in _context.ClasificationDocumentTypes

                join td in _context.Documents
                    on tcd.DocumentTypeId equals td.IdDocumentType

                where tcd.ClassificationAcquisitionId == classificationId

                select new ExpedientDocumentPreviewDto
                {
                    IdDocument = td.IdDocumentType,
                    DocumentName = td.DocumentName
                }

            ).Distinct().ToListAsync();
        }
    }
}
