using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Commands.UpdateProg
{
    public record UpdateProgCommand
     (
        int idProg,
     int Code,
     string Description,
     bool Active
     ) : IRequest<bool>;
}
