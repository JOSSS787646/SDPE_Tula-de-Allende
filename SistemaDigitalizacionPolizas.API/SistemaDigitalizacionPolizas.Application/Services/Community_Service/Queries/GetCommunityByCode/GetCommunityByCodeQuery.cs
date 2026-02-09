using SistemaDigitalizacionPolizas.Domain.Dtos.Community;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Community_Service.Queries.GetCommunityByCode
{
         public record GetCommunityByCodeQuery(int Code)
        : IRequest<CommunityDto>;
}
