using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AppliccationStatus_Service.Commands.CreateApplicationStatus
{
    public record CreateApplicationStatusCommand(
        int Code,
        string Description,
        int Order
    ) : IRequest<int>;
}
