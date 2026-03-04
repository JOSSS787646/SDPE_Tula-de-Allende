using SistemaDigitalizacionPolizas.Domain.Dtos.RequestManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestManager
{
    public interface IRequestManagerRepository
    {
        Task<IEnumerable<RequestManagerPreviewDto>> GetAllAsync();

        Task<int> AddAsync(CreateRequestManagerDto dto);

        Task<bool> UpdateAsync(UpdateRequestManagerDto dto);
    }
}
