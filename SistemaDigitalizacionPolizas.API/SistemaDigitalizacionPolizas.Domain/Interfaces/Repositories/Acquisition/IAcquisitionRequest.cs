using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition
{
    public interface IAcquisitionRequest
    {

        Task<AcquisitionRequest?> AddAsync(AcquisitionRequest request);
        Task<AcquisitionRequestDetailDto?> GetDetailAsync(int idRequest);

        Task<AcquisitionRequest?> GetByIdAsync(int id);

        Task<(IEnumerable<AcquisitionRequestPolizaDto> Data, int TotalRecords)>
            GetAllPolizaInfoPaginatedAsync(int pageNumber, int pageSize);

        Task UpdateAsync(AcquisitionRequest entity);
    }
}
