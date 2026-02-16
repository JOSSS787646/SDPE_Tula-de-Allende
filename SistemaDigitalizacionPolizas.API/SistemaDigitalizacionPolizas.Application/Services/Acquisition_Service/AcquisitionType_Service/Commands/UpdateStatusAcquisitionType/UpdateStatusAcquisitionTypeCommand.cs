using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Commands.UpdateStatusAcquisitionType
{
    public record UpdateStatusAcquisitionTypeCommand
    (int Code,
        bool Active
    ) : IRequest<bool>;
}
