using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateCog;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.UpdateFundingSource
{
    public class updateFundingSourceCommandHandler
        : IRequestHandler<UpdateFundingSourceCommand, bool>
    {
        private readonly IFundingSourceRepository _repository;

        public updateFundingSourceCommandHandler(IFundingSourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateFundingSourceCommand request,
            CancellationToken cancellationToken)
        {
            var fundingSource = new FundingSource

            {
                Code = request.Code,
                Description = request.Description,
                Active = request.Active
            };

            return await _repository.UpdateAsync(fundingSource);
        }
    }
}
