using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionClassification_Service.Commands.UpdateAcquisitionClassification
{
    public record UpdateAcqClassificationCmd(
     int idUpdateAcquisitionClassification,
     int Code,
     string Description,
     bool Active
     ) : IRequest<bool>;
}
