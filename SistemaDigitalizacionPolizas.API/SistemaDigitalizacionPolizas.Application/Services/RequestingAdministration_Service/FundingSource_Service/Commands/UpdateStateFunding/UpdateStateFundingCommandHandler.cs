using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateStateCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.UpdateStatusProyect;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.UpdateStateFunding
{
    public class UpdateStateFundingCommandHandler
        : IRequestHandler<UpdateStateFundingCommand, bool>
    {
        private readonly IFundingSourceRepository _repository;
        private readonly ICurrentUserService _currentUser;
        public UpdateStateFundingCommandHandler(IFundingSourceRepository repository, 
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUser = currentUserService;
        }

        public async Task<bool> Handle(
         UpdateStateFundingCommand request,
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
