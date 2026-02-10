using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Commands.CreateAcquisitionType
{
    public record CreateAcquisitionTypeCommand
    (int Code,
      string Description,
      bool Active
    ) : IRequest<int>;
    
}
