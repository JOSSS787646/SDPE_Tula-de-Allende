using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Queries.GetAllCog;
using SistemaDigitalizacionPolizas.Domain.Dtos.Action;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ActionPolicy_Service.Queries.GetAllActionPolicy
{
    public class GetAllActionPolicyQueryHandler
        : IRequestHandler<GetAllActionPolicyQuery, List<ActionsPolicyDto>>
    {

        private readonly IActionPolicyRepository _actionPolicyRepository;
        public GetAllActionPolicyQueryHandler(IActionPolicyRepository actionPolicyRepository)
        {
            _actionPolicyRepository = actionPolicyRepository;
        }

        public async Task<List<ActionsPolicyDto>> Handle(
      GetAllActionPolicyQuery request,
      CancellationToken cancellationToken)
        {
            var actionsPolicies = await _actionPolicyRepository.GetAllAsync();

            return actionsPolicies.Select(actionPolicy => new ActionsPolicyDto
            {
                IdAction = actionPolicy.IdAction,
                Code = actionPolicy.Code,
                Description = actionPolicy.Description,
                Active = actionPolicy.Active
            }).ToList();
        }
    }
}
