using SistemaDigitalizacionPolizas.Domain.Dtos.ApplicationStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AppliccationStatus_Service.Queries.GetApplicationStatusByCode
{
    public record GetApplicationStatusByCodeQuery(int Code)
       : IRequest<ApplicationStatusDto?>;
}
