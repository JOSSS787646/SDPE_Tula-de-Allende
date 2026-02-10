using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ActionPolicy_Service.Commands.UpdateStatusActionPolicy
{
    public record UpdateStatusActionPolicyCommand
    (int Code, bool Active) : IRequest<bool>;
}
