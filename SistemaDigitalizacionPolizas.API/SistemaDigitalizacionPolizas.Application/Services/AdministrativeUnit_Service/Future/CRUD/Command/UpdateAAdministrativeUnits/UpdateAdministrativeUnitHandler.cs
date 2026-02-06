using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.UpdateAAdministrativeUnits
{
    public class UpdateAdministrativeUnitHandler
        : IRequestHandler<UpdateAdministrativeUnitCommand, bool>
    {
        private readonly IAdministrativeUnit _repository;
        private readonly ICurrentUserService _currentUserService;
        public UpdateAdministrativeUnitHandler(
            IAdministrativeUnit repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(
    UpdateAdministrativeUnitCommand request,
    CancellationToken cancellationToken)
        {
            var unit = await _repository.GetByCodeAsync(request.code);

            if (unit == null)
                return false;

            unit.Active = request.Unit.Active;
            unit.Description = request.Unit.Description;
            unit.UpdatedBy = _currentUserService.UserId;
            unit.UpdatedAt = DateTime.Now;

            return await _repository.UpdateAsync(unit);
        }
    }
}
