using Azure.Core;
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
    }
}
