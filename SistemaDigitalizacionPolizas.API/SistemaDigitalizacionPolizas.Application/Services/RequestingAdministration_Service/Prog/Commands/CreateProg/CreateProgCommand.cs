using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Commands.CreateProg
{
    public record CreateProgCommand
    (
      int Code,
      string Description,
      bool Active
    ): IRequest<int>;
}
