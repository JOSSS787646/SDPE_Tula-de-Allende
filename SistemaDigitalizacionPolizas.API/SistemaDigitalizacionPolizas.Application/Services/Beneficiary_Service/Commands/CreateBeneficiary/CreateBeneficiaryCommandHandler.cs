using SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Commands.CreateBeneficiary;
using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Community;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

public class CreateBeneficiaryCommandHandler
    : IRequestHandler<CreateBeneficiaryCommand, int>
{
    private readonly IBeneficiaryRepository _repository;
    private readonly ICommunityRepository _communityRepository;
    private readonly ICurrentUserService _currentUser;

    public CreateBeneficiaryCommandHandler(
        IBeneficiaryRepository repository,
        ICommunityRepository communityRepository,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _communityRepository = communityRepository;
        _currentUser = currentUser;
    }

    public async Task<int> Handle(
        CreateBeneficiaryCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Beneficiary;

        // Validar CURP
        if (await _repository.ExistsByCurpAsync(dto.Curp))
            throw new InvalidOperationException("Ya existe un beneficiario con esa CURP.");

        // Validar INE
        if (await _repository.ExistsByIneAsync(dto.Ine))
            throw new InvalidOperationException("Ya existe un beneficiario con ese INE.");

        // Validar comunidad si viene informada
        if (dto.IdCommunity.HasValue)
        {
            var community = await _communityRepository
                .GetByIdAsync(dto.IdCommunity.Value);

            if (community == null)
                throw new InvalidOperationException("La comunidad especificada no existe.");
        }

        // 🔥 Mapear DTO → Entidad + Auditoría
        var entity = new Beneficiary
        {
            FirstName = dto.FirstName.Trim(),
            PaternalLastName = dto.PaternalLastName.Trim(),
            MaternalLastName = dto.MaternalLastName?.Trim(),

            Street = dto.Street.Trim(),
            ExternalNumber = dto.ExternalNumber,
            InternalNumber = dto.InternalNumber,
            Neighborhood = dto.Neighborhood,
            PostalCode = dto.PostalCode,
            City = dto.City,
            Municipality = dto.Municipality,
            State = dto.State,
            Country = dto.Country,

            Ine = dto.Ine.Trim().ToUpper(),
            Curp = dto.Curp.Trim().ToUpper(),

            Phone = dto.Phone,
            Email = dto.Email,

            Active = dto.Active,
            idCommunity = dto.IdCommunity,

            // 🔥 Auditoría generada en servidor
            CreatedBy = _currentUser.UserId,
            CreatedAt = DateTime.UtcNow
        };

        var beneficiary = await _repository.AddAsync(entity);

        return beneficiary.IdBeneficiary;
    }
}
