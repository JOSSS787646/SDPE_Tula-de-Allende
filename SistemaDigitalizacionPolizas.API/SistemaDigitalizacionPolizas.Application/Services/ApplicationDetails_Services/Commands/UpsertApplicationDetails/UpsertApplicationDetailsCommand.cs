using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ApplicationDetails_Services.Commands.UpsertApplicationDetails
{
    public record UpsertApplicationDetailsCommand(
     int RequestId,
     List<ApplicationDetailDto> Details
 ) : IRequest;
}
