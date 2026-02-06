using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.UpdateStatusProyect
{
    public record UpdateStatusProyectCommand(
      int Code,
      bool Active
  ) : IRequest<bool>;

}
