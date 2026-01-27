

namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.CreatedAdministrativeUnit;
public class CreateAdministrativeUnitCommandHandler
        : IRequestHandler<CreateAdministrativeUnitCommand, int>
{
    private readonly IAdministrativeUnit _administrativeUnitRepository;

    public CreateAdministrativeUnitCommandHandler(
        IAdministrativeUnit administrativeUnitRepository)
    {
        _administrativeUnitRepository = administrativeUnitRepository;
    }

    public async Task<int> Handle(
        CreateAdministrativeUnitCommand request,
        CancellationToken cancellationToken)
    {
        var unit = new AdministrativeUnit
        {
            Code = request.Code,
            Description = request.Description
        };

        var result = await _administrativeUnitRepository.CreateAsync(unit);

        return result!.IdAdministrativeUnit;
    }
}

