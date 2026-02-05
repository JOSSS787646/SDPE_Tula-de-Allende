using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.UpdateProyect
{
   public record UpdateProyectCommand
    (
     int Code,
     string Description,
     bool Active
     ) : IRequest<bool>;
}
