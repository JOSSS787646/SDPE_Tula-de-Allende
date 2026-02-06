using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Commands.UpdateStatusProg
{
    public record UpdateStatusProgCommand (int Code,
        bool Active
    ) : IRequest<bool>;

}
