using SistemaDigitalizacionPolizas.Domain.Dtos.AdministrativeUnit;

namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.UpdateAAdministrativeUnits
{
    public record UpdateAdministrativeUnitCommand(
     int code,
     AdministrativeUnitDto Unit
 ) : IRequest<bool>;

}
