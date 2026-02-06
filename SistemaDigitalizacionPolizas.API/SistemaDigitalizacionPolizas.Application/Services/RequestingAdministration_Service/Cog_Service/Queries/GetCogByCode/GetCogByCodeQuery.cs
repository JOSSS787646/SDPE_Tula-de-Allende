using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Queries.GetCogByCode
{
    public record GetCogByCodeQuery(int Code)
        : IRequest<COGDto?>;
    
}
