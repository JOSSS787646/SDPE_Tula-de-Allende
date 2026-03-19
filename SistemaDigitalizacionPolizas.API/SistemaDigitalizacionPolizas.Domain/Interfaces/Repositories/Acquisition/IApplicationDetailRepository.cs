using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition
{
    public interface IApplicationDetailRepository
    {

        Task<List<ApplicationDetail>> GetByRequestIdAsync(int requestId);

        Task<ApplicationDetail?> GetByIdAsync(int detailId);

        Task<int> AddAsync(ApplicationDetail detail);

        Task UpdateAsync(ApplicationDetail detail);

        Task DeleteAsync(int detailId);

        Task UpsertRangeAsync(int requestId, List<ApplicationDetail> details);
    }
}
