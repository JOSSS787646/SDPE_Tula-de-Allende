using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateCog;
using SistemaDigitalizacionPolizas.Domain.Entities.Actions_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Actions;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ActionPolicy_Service.Commands.UpdateActionPolicy
{
    public class UpdateActionPolicyCommandHandler
        : IRequestHandler<UpdateActionPolicyCommand, bool>
    {
        private readonly IActionPolicyRepository _repository;
        private readonly ICurrentUserService _currentUserService;


        public UpdateActionPolicyCommandHandler(IActionPolicyRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(
         UpdateActionPolicyCommand request,
         CancellationToken cancellationToken)
        {
            var action = new ActionsPolicy
            {
                Code = request.Code,
                Description = request.Description,
                Active = request.Active,



                UpdatedBy = _currentUserService.UserId,
                UpdatedAt = DateTime.Now
            };

            return await _repository.UpdateAsync(action);
        }
    }
}
