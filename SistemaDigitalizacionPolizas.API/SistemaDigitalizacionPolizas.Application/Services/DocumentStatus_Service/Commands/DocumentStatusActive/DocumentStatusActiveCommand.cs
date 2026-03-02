using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Commands.DocumentStatusActive
{
    public record DocumentStatusActiveCommand(
        int Code,
        bool Active
    ) : IRequest<bool>;
}
