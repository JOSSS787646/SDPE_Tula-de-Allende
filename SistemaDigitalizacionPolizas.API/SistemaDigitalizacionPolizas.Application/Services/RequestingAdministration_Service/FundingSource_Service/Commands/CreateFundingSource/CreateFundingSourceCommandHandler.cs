using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.CreateCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.AddFundingSource;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.CreateFundingSource
{
    public class CreateFundingSourceCommandHandler
         : IRequestHandler<CreateFundingSourceCommand, int>
    {
        private readonly IFundingSourceRepository _repository;

        public CreateFundingSourceCommandHandler(IFundingSourceRepository repository)
        {
            _repository = repository;
        }
        public async Task<int> Handle(
            CreateFundingSourceCommand request,
            CancellationToken cancellationToken)
        {
            var fundingSource = new FundingSource
            {
                Code = request.Code,
                Description = request.Description,
                Active = request.Active
            };
            var result = await _repository.AddAsync(fundingSource);
            if (result == null)
                throw new InvalidOperationException(
                    $"Ya existe una Fuente de Financiamiento con el código {request.Code}");
            return result.idFundingSource;
        }
    }

}
