
using SistemaDigitalizacionPolizas.Domain.Dtos.AdministrativeUnit;

namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Queries.GetAllAdministrativeUnit
{
    public record GetAllAdministrativeUnitQuery()
      : IRequest<List<AdministrativeUnitDto>>;
}
