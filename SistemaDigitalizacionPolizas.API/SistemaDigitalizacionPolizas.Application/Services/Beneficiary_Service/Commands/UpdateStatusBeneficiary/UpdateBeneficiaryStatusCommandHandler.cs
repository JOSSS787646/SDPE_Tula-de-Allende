using MediatR;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Community;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Commands.UpdateStatusBeneficiary
{
    internal class UpdateBeneficiaryStatusCommandHandler
        : IRequestHandler<UpdateBeneficiaryStatusCommand, bool>
    {
        private readonly IBeneficiaryRepository _repository;

        public UpdateBeneficiaryStatusCommandHandler(IBeneficiaryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateBeneficiaryStatusCommand request,
            CancellationToken cancellationToken)
        {
            var curp = request.Curp.Trim().ToUpper();

            var entity = await _repository.GetByCurpAsync(curp);

            if (entity is null)
                throw new KeyNotFoundException("Beneficiario no encontrado por CURP.");

            entity.Active = request.Active;
            entity.ModifiedAt = DateTime.UtcNow;

            return await _repository.UpdateAsync(entity);
        }
    }
}
