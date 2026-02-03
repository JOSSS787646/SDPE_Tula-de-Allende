namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.DeleteAdministrativeUnit
{
    public record DeleteAdministrativeUnitCommand(int Code)
        : IRequest<bool>;
}
