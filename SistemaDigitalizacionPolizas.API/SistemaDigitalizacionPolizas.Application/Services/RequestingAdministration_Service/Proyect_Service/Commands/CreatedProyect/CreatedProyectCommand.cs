using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.CreatedProyect
{
    public record CreatedProyectCommand(
      int Code,
      string Description,
      bool Active
  ) : IRequest<int>;

}
