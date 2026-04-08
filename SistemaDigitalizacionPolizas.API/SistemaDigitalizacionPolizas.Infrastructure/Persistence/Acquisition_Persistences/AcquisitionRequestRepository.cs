using Azure.Core;
using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Acquisition_Persistences
{
    public class AcquisitionRequestRepository : IAcquisitionRequest
    {
        private readonly SdpeDbContext _context;

        public async Task<List<AcquisitionRequest>> GetByClassificationId(int classificationId)
        {
            return await _context.AcquisitionRequests
                .Where(x => x.IdAcquisitionClassification == classificationId)
                .ToListAsync();
        }

        public AcquisitionRequestRepository(SdpeDbContext context)
        {
            _context = context;
        }

        public async Task<AcquisitionRequest?> AddAsync(AcquisitionRequest request)
        {
            await _context.AcquisitionRequests.AddAsync(request);
            await _context.SaveChangesAsync();

            return request;
        }

        public async Task<AcquisitionRequest?> GetByIdAsync(int id)
        {
            return await _context.AcquisitionRequests
                .AsNoTracking() 
                .FirstOrDefaultAsync(x => x.IdRequest == id);
        }

        public async Task RemovePaymentPolicyFromRequests(int paymentPolicyId)
        {
            var requests = await _context.AcquisitionRequests
                .Where(x => x.IdPaymentPolicy == paymentPolicyId)
                .ToListAsync();

            foreach (var req in requests)
            {
                req.IdPaymentPolicy = null;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByRequestNumberAsync(string requestNumber)
        {
            return await _context.AcquisitionRequests
                .AnyAsync(x => x.RequestNumber == requestNumber);
        }

        public async Task UpdateAsync(AcquisitionRequest entity)
        {
            _context.AcquisitionRequests.Update(entity); 
            await _context.SaveChangesAsync();
        }
        public async Task UpdateMaxDateAsync(int requestId, DateTime newDate)
        {
            var rows = await _context.AcquisitionRequests
                .Where(x => x.IdRequest == requestId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.CompleteMaximeDate, newDate));

            if (rows == 0)
                throw new Exception("No existe");
        }



        public async Task UpdateStatusAsync(int requestId, int newStatus)
        {
            await _context.AcquisitionRequests
                .Where(x => x.IdRequest == requestId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.IdApplicationStatus, newStatus));
        }


        public async Task<AcquisitionRequestStatusDto?> GetStatusDataAsync(int requestId)
        {
            return await _context.AcquisitionRequests
                .Where(x => x.IdRequest == requestId)
                .Select(x => new AcquisitionRequestStatusDto
                {
                    IdRequest = x.IdRequest,
                    IdAcquisitionClassification = x.IdAcquisitionClassification,
                    IdApplicationStatus = x.IdApplicationStatus,
                    CompleteMaximeDate = x.CompleteMaximeDate
                })
                .FirstOrDefaultAsync();
        }



        public async Task<AcquisitionRequestDetailDto?> GetDetailAsync(int idRequest)
        {
            return await _context.AcquisitionRequests
                .AsNoTracking()
                .Where(x => x.IdRequest == idRequest)
                .Select(x => new AcquisitionRequestDetailDto
                {
                    IdRequest = x.IdRequest,
                    RequestNumber = x.RequestNumber,
                    RequestDate = x.RequestDate,
                    Justification = x.Justification,
                    AuthorizationDate = x.AuthorizationDate,
                    Observations = x.Observations,
                    CompleteMaximeDate = x.CompleteMaximeDate,

                    PolicyNumber = x.PaymentPolicy != null
                        ? x.PaymentPolicy.PolicyCode
                        : null,

                    CFDI = x.CFDI,

                    AdministrativeUnit = x.AdministrativeUnit == null ? null : new SimpleCatalogDto
                    {
                        Id = x.AdministrativeUnit.IdAdministrativeUnit,
                        Name = x.AdministrativeUnit.Description
                    },

                    Proyect = x.Project == null ? null : new SimpleCatalogDto
                    {
                        Id = x.Project.idProyect,
                        Name = x.Project.Description
                    },

                    AcquisitionType = x.AcquisitionType == null ? null : new SimpleCatalogDto
                    {
                        Id = x.AcquisitionType.idAcquisitionType,
                        Name = x.AcquisitionType.Description
                    },

                    Supplier = x.Supplier == null ? null : new SimpleCatalogDto
                    {
                        Id = x.Supplier.IdSupplier,
                        Name = x.Supplier.ContactName
                    },

                    ApplicationStatus = x.ApplicationStatus == null ? null : new SimpleCatalogDto
                    {
                        Id = x.ApplicationStatus.IdApplicationStatus,
                        Name = x.ApplicationStatus.Description
                    },

                    FundingSource = x.FundingSource == null ? null : new SimpleCatalogDto
                    {
                        Id = x.FundingSource.idFundingSource,
                        Name = x.FundingSource.Description
                    },

                    AcquisitionClassification = x.AcquisitionClassification == null ? null : new SimpleCatalogDto
                    {
                        Id = x.AcquisitionClassification.idAcquisitionClassification,
                        Name = x.AcquisitionClassification.Description
                    },

                    Program = x.Program == null ? null : new SimpleCatalogDto
                    {
                        Id = x.Program.idProg,
                        Name = x.Program.Description
                    },

                    Community = x.Community == null ? null : new SimpleCatalogDto
                    {
                        Id = x.Community.idCommunity,
                        Name = x.Community.Description
                    },

                    Beneficiary = x.Beneficiary == null ? null : new SimpleCatalogDto
                    {
                        Id = x.Beneficiary.IdBeneficiary,
                        Name = $"{x.Beneficiary.FirstName} {x.Beneficiary.PaternalLastName} {x.Beneficiary.MaternalLastName}"
                    },

                    CurrentRequestStatus = x.ApplicationStatus == null ? null : new SimpleCatalogDto
                    {
                        Id = x.ApplicationStatus.IdApplicationStatus,
                        Name = x.ApplicationStatus.Description
                    },

                    // 🔥🔥🔥 AQUÍ LO NUEVO → DETALLES
                    Details = x.Details.Select(d => new ApplicationDetailDto
                    {
                        IdDetail = d.IdDetail,

                        // 🔥 COG con nombre
                        Cog = d.Cog == null ? null : new SimpleCatalogDto
                        {
                            Id = d.Cog.idCog,
                            Name = d.Cog.Description
                        },

                        Quantity = d.Quantity,
                        UnitMeasure = d.UnitMeasure,
                        Description = d.Description,
                        UnitAmount = d.UnitAmount,
                        TotalAmount = d.TotalAmount

                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }


        public async Task<(IEnumerable<AcquisitionRequestPolizaDto> Data, int TotalRecords)>GetAllPolizaInfoPaginatedAsync(int pageNumber, int pageSize)
        {
            var query = _context.AcquisitionRequests
                .AsNoTracking();

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new AcquisitionRequestPolizaDto
                {
                    IdRequest = r.IdRequest,
                    Folio = r.RequestNumber,
                    CFDI=r.CFDI,

                    AcquisitionClassification = r.AcquisitionClassification != null
                        ? r.AcquisitionClassification.Description
                        : "Sin clasificación",

                    RequestDate = r.RequestDate,

                    Status = r.ApplicationStatus != null
                        ? r.ApplicationStatus.Description
                        : "Sin estatus",

                    PolicyNumber = r.PaymentPolicy != null
                        ? r.PaymentPolicy.PolicyCode
                        : null,

                    CompleteMaximeDate = r.CompleteMaximeDate != null
                        ? r.CompleteMaximeDate
                    : null
                })
                .ToListAsync();

            return (data, totalRecords);
        }

        public async Task DeleteCascadeAsync(int solicitudId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // ==========================================
                // 1️⃣ Obtener la póliza asociada (si la necesitas después)
                // ==========================================
                var policyId = await _context.AcquisitionRequests
                    .Where(x => x.IdRequest == solicitudId)
                    .Select(x => x.IdPaymentPolicy)
                    .FirstOrDefaultAsync();

                // ==========================================
                // 2️⃣ ELIMINAR DETALLES 🔥 (LO QUE TE FALTABA)
                // ==========================================
                await _context.ApplicationDetails
                    .Where(x => x.ApplicationId == solicitudId)
                    .ExecuteDeleteAsync();

                // ==========================================
                // 3️⃣ Documentos
                // ==========================================
                await _context.ExpedientDocuments
                    .Where(x => x.RequestId == solicitudId)
                    .ExecuteDeleteAsync();

                // ==========================================
                // 4️⃣ Managers
                // ==========================================
                await _context.RequestManagers
                    .Where(x => x.IdRequest == solicitudId)
                    .ExecuteDeleteAsync();

                // ==========================================
                // 5️⃣ Excepciones
                // ==========================================
                await _context.RequestDocumentExceptions
                    .Where(x => x.IdRequest == solicitudId)
                    .ExecuteDeleteAsync();

                // ==========================================
                // 6️⃣ Finalmente la solicitud
                // ==========================================
                await _context.AcquisitionRequests
                    .Where(x => x.IdRequest == solicitudId)
                    .ExecuteDeleteAsync();

                // ==========================================
                // COMMIT
                // ==========================================
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<AcquisitionRequest?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.AcquisitionRequests
                .AsNoTracking()
                .Include(x => x.AdministrativeUnit)
                .Include(x => x.Program)
                .Include(x => x.Project)
                .Include(x => x.FundingSource)
                .Include(x => x.AcquisitionType)
                .Include(x => x.AcquisitionClassification)
                .Include(x => x.Supplier)
                .Include(x => x.PaymentPolicy)
                .Include(x => x.Community)
                .Include(x => x.Beneficiary)
                .Include(x => x.ApplicationStatus)
                .Include(x => x.Details)
                    .ThenInclude(d => d.Cog)
                .FirstOrDefaultAsync(x => x.IdRequest == id);
        }


        public async Task<List<ExpiredRequestDto>> GetExpiredRequestsAsync()
        {
            var today = DateTime.UtcNow;

            var data = await (
                from s in _context.AcquisitionRequests

                join de in _context.ExpedientDocuments
                    on s.IdRequest equals de.RequestId into deGroup
                from de in deGroup.DefaultIfEmpty()

                join ed in _context.DocumentStatuses
                    on de.IdDocumentStatus equals ed.idDocumentStatus into edGroup
                from ed in edGroup.DefaultIfEmpty()

                join td in _context.Documents
                    on de.DocumentTypeId equals td.IdDocumentType into tdGroup
                from td in tdGroup.DefaultIfEmpty()

                join e in _context.RequestManagers
                    on s.IdRequest equals e.IdRequest into em
                from e in em.DefaultIfEmpty()

                where s.CompleteMaximeDate < today

                    // 🔥 FILTRO SEGURO
                    && (ed == null || ed.Description != "Aprobado")

                    && (
                        s.LastNotificationSentAt == null ||
                        s.LastNotificationSentAt <= today.AddDays(-7)
                    )

                select new
                {
                    s.IdRequest,
                    s.RequestNumber,
                    s.LastNotificationSentAt,

                    UserEmail = e != null ? (e.Email ?? "") : "",
                    UserName = e != null ? (e.FirstName ?? "Usuario") : "Usuario",

                    DocumentName = td != null ? (td.DocumentName ?? "Documento") : "Documento",
                    Status = ed != null ? (ed.Description ?? "Pendiente") : "Pendiente"
                }
            ).ToListAsync();

            var filtered = data
                .Where(x => !string.IsNullOrEmpty(x.UserEmail))
                .ToList();

            var result = filtered
                .GroupBy(x => new { x.IdRequest, x.UserEmail })
                .Select(g => new ExpiredRequestDto
                {
                    RequestId = g.Key.IdRequest,
                    RequestNumber = g.First().RequestNumber,
                    UserEmail = g.Key.UserEmail,
                    UserName = g.First().UserName,
                    LastNotificationSentAt = g.First().LastNotificationSentAt,

                    MissingDocuments = g.Select(d => new ExpiredRequestDto.MissingDocument
                    {
                        DocumentName = d.DocumentName,
                        Status = d.Status
                    }).ToList()
                })
                .ToList();

            return result;
        }

        public async Task UpdateNotificationMetadataAsync(int requestId)
        {
            var request = await _context.AcquisitionRequests
                .Where(x => x.IdRequest == requestId)
                .Select(x => new
                {
                    x.IdRequest,
                    NotificationCount = (int?)x.NotificationCount ?? 0 // Cast a nullable
                })
                .FirstOrDefaultAsync();

            if (request == null)
                return;

            var entity = new AcquisitionRequest
            {
                IdRequest = request.IdRequest,
                LastNotificationSentAt = DateTime.UtcNow,
                NotificationCount = request.NotificationCount + 1
            };

            _context.AcquisitionRequests.Attach(entity);
            _context.Entry(entity).Property(x => x.LastNotificationSentAt).IsModified = true;
            _context.Entry(entity).Property(x => x.NotificationCount).IsModified = true;

            await _context.SaveChangesAsync();
        }

    }
}
