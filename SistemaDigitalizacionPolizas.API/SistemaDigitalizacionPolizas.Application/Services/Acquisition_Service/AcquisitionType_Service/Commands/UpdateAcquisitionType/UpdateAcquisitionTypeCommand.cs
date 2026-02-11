using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Commands.UpdateAcquisitionType
{
    public record UpdateAcquisitionTypeCommand
    (
    int idUpdateAcquisitionType,
     int Code,
     string Description,
     bool Active
     ) : IRequest<bool>;
}
