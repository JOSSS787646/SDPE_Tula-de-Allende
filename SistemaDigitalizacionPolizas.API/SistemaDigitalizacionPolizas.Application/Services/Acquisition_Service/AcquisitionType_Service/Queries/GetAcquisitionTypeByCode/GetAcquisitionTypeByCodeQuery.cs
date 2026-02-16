using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitonType;
using SistemaDigitalizacionPolizas.Domain.Dtos.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Queries.GetAcquisitionTypeByCode
{
    public record GetAcquisitionTypeByCodeQuery(int Code)
        : IRequest<AcquisitionTypeDto>;
  
}
