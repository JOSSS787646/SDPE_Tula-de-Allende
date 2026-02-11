using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Queries.CreateQuery;
using SistemaDigitalizacionPolizas.Domain.Dtos.Community;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Community;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Community_Service.Queries.GetAllCommunity
{
    public class GetAllCommunityQueryHandler
        : IRequestHandler<GetAllCommunityQuery, List<CommunityDto>>

    {

        private readonly ICommunityRepository _communityRepository;

        public GetAllCommunityQueryHandler(ICommunityRepository repository)
        {
            _communityRepository = repository;
        }

        public async Task<List<CommunityDto>> Handle(
GetAllCommunityQuery request,
CancellationToken cancellationToken)
        {
            var communities = await _communityRepository.GetAllAsync();

            return communities.Select(community=> new CommunityDto
            {
                idCommunity = community.idCommunity,
                Code = community.Code,
                Description = community.Description,
                Active = community.Active
            }).ToList();
        }
    }
}
