using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ActionPolicy_Service.Commands.CreateActionPolicy
{
    public record CreateActionPolicyCommand
    (
        int Code,
        string Description,
        bool Active
    ) : IRequest<int>;
}
