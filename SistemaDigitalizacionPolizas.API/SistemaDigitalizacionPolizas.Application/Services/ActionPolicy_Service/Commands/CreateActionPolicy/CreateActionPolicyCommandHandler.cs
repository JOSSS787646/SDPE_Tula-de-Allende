using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.CreateCog;
using SistemaDigitalizacionPolizas.Domain.Entities.Actions_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Actions;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ActionPolicy_Service.Commands.CreateActionPolicy
{
    public class CreateActionPolicyCommandHandler
        : IRequestHandler<CreateActionPolicyCommand, int>
    {
        private readonly IActionPolicyRepository _repository;
        private readonly ICurrentUserService _currentUserService;


        public CreateActionPolicyCommandHandler(IActionPolicyRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(
            CreateActionPolicyCommand request,
            CancellationToken cancellationToken)
        {
            var action = new ActionsPolicy
            {
                Code = request.Code,
                Description = request.Description,
                Active = request.Active,


                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTime.Now
            };

            var result = await _repository.AddAsync(action);

            if (result == null)
                throw new InvalidOperationException(
                    $"Ya existe un COG con el código {request.Code}");

            return result.IdAction;
        }
    }
}
