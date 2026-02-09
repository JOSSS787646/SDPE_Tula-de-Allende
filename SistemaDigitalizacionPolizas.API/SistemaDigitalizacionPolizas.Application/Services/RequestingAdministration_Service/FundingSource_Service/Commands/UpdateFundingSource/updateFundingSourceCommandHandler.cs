using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateCog;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
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
        private readonly ICurrentUserService _currentUser;      
        public updateFundingSourceCommandHandler(IFundingSourceRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUser = currentUserService;
        }

        public async Task<bool> Handle(
     UpdateFundingSourceCommand request,
     CancellationToken cancellationToken)
        {
            // 1️⃣ Buscar por ID
            var fundingSource = await _repository.GetByIdAsync(request.idFundingSource);

            if (fundingSource == null)
                return false;

            // 2️⃣ Modificar
            fundingSource.Code = request.Code;
            fundingSource.Description = request.Description;
            fundingSource.Active = request.Active;
            fundingSource.UpdatedBy = _currentUser.UserId;
            fundingSource.UpdatedAt = DateTime.Now;

            // 3️⃣ Guardar
            return await _repository.UpdateAsync(fundingSource);
        }
    }
}
