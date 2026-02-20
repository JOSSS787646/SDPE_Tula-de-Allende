using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AppliccationStatus_Service.Commands.UpdateApplicationStatus
{
    public record UpdateApplicationStatusStateCommand(
            int Code,
        bool Active
    ) : IRequest<bool>;
}
