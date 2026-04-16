using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.CreateCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.AddFundingSource;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
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
        private readonly ICurrentUserService _currentUserService;
        public CreateFundingSourceCommandHandler(IFundingSourceRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }
        public async Task<int> Handle(
            CreateFundingSourceCommand request,
            CancellationToken cancellationToken)
        {
            var fundingSource = new FundingSource
            {
                Code = request.Code,
                Description = request.Description,
                Active = request.Active,



                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTime.UtcNow
            };
            var result = await _repository.AddAsync(fundingSource);
            if (result == null)
                throw new InvalidOperationException(
                    $"Ya existe una Fuente de Financiamiento con el código {request.Code}");
            return result.idFundingSource;
        }
    }

}
