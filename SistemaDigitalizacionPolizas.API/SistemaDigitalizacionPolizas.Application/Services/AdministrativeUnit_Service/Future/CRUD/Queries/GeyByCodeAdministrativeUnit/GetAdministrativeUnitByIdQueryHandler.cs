using SistemaDigitalizacionPolizas.Domain.Dtos.AdministrativeUnit;

namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Queries.GeyByIdAdministrativeUnit
{
    public class GetAdministrativeUnitByIdQueryHandler
    : IRequestHandler<GetAdministrativeUnitByCodeQuery, AdministrativeUnitDto?>
    {
        private readonly IAdministrativeUnit _repository;

        public GetAdministrativeUnitByIdQueryHandler(
            IAdministrativeUnit repository)
        {
            _repository = repository;
        }

        public async Task<AdministrativeUnitDto?> Handle(
            GetAdministrativeUnitByCodeQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByCodeAsync(request.Code);

            if (entity == null)
                return null;

            return new AdministrativeUnitDto
            {
                IdAdministrativeUnit = entity.IdAdministrativeUnit,
                Code = entity.Code,
                Description = entity.Description
            };
        }
    }
}
