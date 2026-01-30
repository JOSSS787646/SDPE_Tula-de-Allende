using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateCog
{
    public record UpdateCogCommand(
     int Code,
     string Description,
     bool Active
 ) : IRequest<bool>;


}
