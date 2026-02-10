using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Queries.GetCogByCode;
using SistemaDigitalizacionPolizas.Domain.Dtos.Action;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ActionPolicy_Service.Queries.GetActionPolicyByCode
{
    public class GetActionPolicyByCodeQueryHandler
         : IRequestHandler<GetActionPolicyByCodeQuery, ActionsPolicyDto?>
    {

        private readonly IActionPolicyRepository _actionPolicyRepository;

        public GetActionPolicyByCodeQueryHandler(IActionPolicyRepository actionPolicyRepository)
        {
            _actionPolicyRepository = actionPolicyRepository;
        }

        public async Task<ActionsPolicyDto?> Handle(
    GetActionPolicyByCodeQuery request,
    CancellationToken cancellationToken)
        {
            var actionPolicy = await _actionPolicyRepository.GetByCodeAsync(request.Code);
            if (actionPolicy == null)
                return null;

            return new ActionsPolicyDto
            {
                IdAction = actionPolicy.IdAction,
                Code = actionPolicy.Code,
                Description = actionPolicy.Description,
                Active = actionPolicy.Active
            };
        }
    }
}
