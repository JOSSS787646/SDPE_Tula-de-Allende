using SistemaDigitalizacionPolizas.Domain.Dtos.AdministrativeUnit;

namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Queries.GeyByIdAdministrativeUnit
{
    public record GetAdministrativeUnitByCodeQuery(int Code)
  : IRequest<AdministrativeUnitDto?>;


}
