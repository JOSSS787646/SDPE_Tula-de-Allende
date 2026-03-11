using SistemaDigitalizacionPolizas.Domain.Dtos.RequestManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.CreateRequestManager_Service.Queries.GetAllRequestManagers
{
    public record GetAllRequestManagersQuery()
    : IRequest<IEnumerable<RequestManagerPreviewDto>>;
}
