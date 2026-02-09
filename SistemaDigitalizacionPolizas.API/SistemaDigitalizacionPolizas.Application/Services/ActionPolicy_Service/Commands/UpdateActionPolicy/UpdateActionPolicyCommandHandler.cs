
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Commands.UpdateProg;
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
            // 1️⃣ Buscar por ID
            var actionsPolicy = await _repository.GetByIdAsync(request.idActionPolicy);

            if (actionsPolicy == null)
                return false;

            // 2️⃣ Modificar
            actionsPolicy.Code = request.Code;
            actionsPolicy.Description = request.Description;
            actionsPolicy.Active = request.Active;
            actionsPolicy.UpdatedBy = _currentUserService.UserId;
            actionsPolicy.UpdatedAt = DateTime.Now;

            // 3️⃣ Guardar
            return await _repository.UpdateAsync(actionsPolicy);
        }
    }
}

