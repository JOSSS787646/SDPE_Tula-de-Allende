using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.UpdateStateFunding;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.UpdateStatusProyect
{
    public class UpdateStatusProyectCommandHandler
        : IRequestHandler<UpdateStatusProyectCommand, bool>
    {
        private readonly IProyectRepository _repository;
        private readonly ICurrentUserService _currentUser;
        public UpdateStatusProyectCommandHandler(IProyectRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
          UpdateStatusProyectCommand request,
          CancellationToken cancellationToken)
        {
            var proyect = await _repository.GetByCodeAsync(request.Code);

            if (proyect == null)
                return false;    
            proyect.Active = request.Active;
            proyect.UpdatedBy = _currentUser.UserId;
            proyect.UpdatedAt = DateTime.Now;

            return await _repository.UpdateAsync(proyect);
        }
    }
}
