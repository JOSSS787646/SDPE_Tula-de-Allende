using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitonType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionClassification_Service.Queries.GetAllAcqClassification
{
    public record GetAllAcqClassificationQuery
   () : IRequest<List<AcquisitionClassificationDto>>;
}
