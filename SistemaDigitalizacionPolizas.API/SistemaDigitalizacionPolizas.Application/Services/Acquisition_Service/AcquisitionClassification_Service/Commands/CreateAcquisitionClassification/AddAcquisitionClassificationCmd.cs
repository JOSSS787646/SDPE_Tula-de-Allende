using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionClassification_Service.Commands.CreateAcquisitionClassification
{
    public record AddAcquisitionClassificationCmd
        (int Code,
      string Description,
      bool Active
    ) : IRequest<int>;

}

