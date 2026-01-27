

namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.CreatedAdministrativeUnit
{
    public record CreateAdministrativeUnitCommand(
     int Code,
     string Description
 ) : IRequest<int>;
}
