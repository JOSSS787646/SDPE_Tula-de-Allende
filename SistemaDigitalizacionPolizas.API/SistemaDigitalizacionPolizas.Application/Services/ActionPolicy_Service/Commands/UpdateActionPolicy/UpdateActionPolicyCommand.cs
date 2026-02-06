using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ActionPolicy_Service.Commands.UpdateActionPolicy
{
    public record UpdateActionPolicyCommand
    (
     int Code,
     string Description,
     bool Active
 ) : IRequest<bool>;
}
