using SistemaDigitalizacionPolizas.Domain.Dtos.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ActionPolicy_Service.Queries.GetActionPolicyByCode
{
    public record GetActionPolicyByCodeQuery (int Code) 
        : IRequest<ActionsPolicyDto>;

}
