using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateStateCog;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
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

        public UpdateStateFundingCommandHandler(IFundingSourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
          UpdateStateFundingCommand request,
          CancellationToken cancellationToken)
        {
            var cog = await _repository.GetByCodeAsync(request.Code);

            if (cog == null)
                return false;

            cog.Active = false;

            return await _repository.UpdateAsync(cog);
        }
    }
}
