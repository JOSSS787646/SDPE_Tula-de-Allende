using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Queries.GetProyectByCode
{
    public record GetProyectByCodeQuery(int code)
     : IRequest<ProyectDto?>;
}
