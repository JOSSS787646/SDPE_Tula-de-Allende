using SistemaDigitalizacionPolizas.Domain.Dtos.Action;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ActionPolicy_Service.Queries.GetAllActionPolicy
{
    public record GetAllActionPolicyQuery
    (): IRequest<List<ActionsPolicyDto>>;
}
