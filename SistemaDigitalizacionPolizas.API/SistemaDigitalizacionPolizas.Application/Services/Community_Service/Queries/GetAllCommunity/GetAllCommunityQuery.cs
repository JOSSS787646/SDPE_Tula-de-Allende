using SistemaDigitalizacionPolizas.Domain.Dtos.Community;

namespace SistemaDigitalizacionPolizas.Application.Services.Community_Service.Queries.GetAllCommunity
{
    public class GetAllCommunityQuery
       () : IRequest<List<CommunityDto>>;
}
