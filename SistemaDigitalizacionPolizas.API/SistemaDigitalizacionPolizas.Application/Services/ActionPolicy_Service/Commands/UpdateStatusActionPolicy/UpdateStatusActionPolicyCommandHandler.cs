using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateStateCog;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Actions;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ActionPolicy_Service.Commands.UpdateStatusActionPolicy
{
    public class UpdateStatusActionPolicyCommandHandler
        : IRequestHandler<UpdateStatusActionPolicyCommand, bool>
    {
        private readonly IActionPolicyRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateStatusActionPolicyCommandHandler(IActionPolicyRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(
   UpdateStatusActionPolicyCommand request,
   CancellationToken cancellationToken)
        {
            var proyect = await _repository.GetByCodeAsync(request.Code);

            if (proyect == null)
                return false;


            proyect.Active = request.Active;

            // Auditoría
            proyect.UpdatedBy = _currentUserService.UserId;
            proyect.UpdatedAt = DateTime.Now;

            return await _repository.UpdateAsync(proyect);
        }
    }
}
