

using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.CreatedAdministrativeUnit;
public class CreateAdministrativeUnitCommandHandler
        : IRequestHandler<CreateAdministrativeUnitCommand, int>
{
    private readonly IAdministrativeUnit _administrativeUnitRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateAdministrativeUnitCommandHandler(
        IAdministrativeUnit administrativeUnitRepository, ICurrentUserService currentUserService
        )
    {
        _administrativeUnitRepository = administrativeUnitRepository;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(
        CreateAdministrativeUnitCommand request,
        CancellationToken cancellationToken)
    {
        var unit = new AdministrativeUnit
        {
            Code = request.Code,
            Description = request.Description,

            CreatedBy = _currentUserService.UserId,
            CreatedAt = DateTime.Now
        };


        var result = await _administrativeUnitRepository.CreateAsync(unit);

        return result!.IdAdministrativeUnit;
    }
}

