using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateStateCog;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.DeleteAdministrativeUnit
{
    public class DeleteAdministrativeUnitCommandHandler
         : IRequestHandler<DeleteAdministrativeUnitCommand, bool>
    {
        private readonly IAdministrativeUnit _repository;
        private readonly ICurrentUserService _currentUser;

        public DeleteAdministrativeUnitCommandHandler(IAdministrativeUnit repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUser = currentUserService;
        }

        public async Task<bool> Handle(
      DeleteAdministrativeUnitCommand request,
      CancellationToken cancellationToken)
        {
            var administrativeUnit = await _repository.GetByCodeAsync(request.Code);

            if (administrativeUnit == null)
                return false;


            administrativeUnit.Active = request.Active;

            // Auditoría
            administrativeUnit.UpdatedBy = _currentUser.UserId;
            administrativeUnit.UpdatedAt = DateTime.Now;

            return await _repository.UpdateAsync(administrativeUnit);
        }
    }
}
