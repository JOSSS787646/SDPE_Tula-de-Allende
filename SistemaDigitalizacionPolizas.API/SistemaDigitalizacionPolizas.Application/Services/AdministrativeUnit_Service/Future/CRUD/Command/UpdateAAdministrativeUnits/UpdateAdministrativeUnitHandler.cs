using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.UpdateAAdministrativeUnits
{
    public class UpdateAdministrativeUnitHandler
        : IRequestHandler<UpdateAdministrativeUnitCommand, bool>
    {
        private readonly IAdministrativeUnit _repository;

        public UpdateAdministrativeUnitHandler(
            IAdministrativeUnit repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
    UpdateAdministrativeUnitCommand request,
    CancellationToken cancellationToken)
        {
            var unit = await _repository.GetByCodeAsync(request.code);

            if (unit == null)
                return false;

            unit.Description = request.Unit.Description;

            return await _repository.UpdateAsync(unit);
        }
    }
}
