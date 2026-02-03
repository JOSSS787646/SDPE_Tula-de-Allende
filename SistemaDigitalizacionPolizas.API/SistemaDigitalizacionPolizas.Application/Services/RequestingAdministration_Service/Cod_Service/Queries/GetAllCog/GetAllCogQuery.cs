using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Queries.GetAllCog
{
    public record GetAllCogQuery()
        : IRequest<List<COGDto>>;

}
