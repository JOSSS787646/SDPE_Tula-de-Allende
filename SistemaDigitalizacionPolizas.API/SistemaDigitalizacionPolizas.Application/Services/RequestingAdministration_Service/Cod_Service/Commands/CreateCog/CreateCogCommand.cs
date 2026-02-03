using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.CreateCog
{
    public record CreateCogCommand(
        int Code,
        string Description,
        bool Active
    ) : IRequest<int>;
    
}
