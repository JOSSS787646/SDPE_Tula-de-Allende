using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionClassification_Service.Commands.UpdateStatusAcqClassification
{
    public record UpdateStatusAcqClassificationCmd
      (int Code,
        bool Active
    ) : IRequest<bool>;
}
