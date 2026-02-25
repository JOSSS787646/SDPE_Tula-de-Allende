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
    public class AcquisitionRequestRepository: IAcquisitionRequest
    {
        private readonly SdpeDbContext _context;

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
                })
                .FirstOrDefaultAsync();
        }
    }
}
