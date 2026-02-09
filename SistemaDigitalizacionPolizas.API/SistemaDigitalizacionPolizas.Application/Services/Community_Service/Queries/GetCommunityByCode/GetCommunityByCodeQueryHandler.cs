using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Queries.GetByCodeProg;
using SistemaDigitalizacionPolizas.Domain.Dtos.Community;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Communitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Community_Service.Queries.GetCommunityByCode
{
    public class GetCommunityByCodeQueryHandler
        : IRequestHandler<GetCommunityByCodeQuery, CommunityDto>
    {

        private readonly ICommunityRepository _communityRepository;

        public GetCommunityByCodeQueryHandler(ICommunityRepository repository)
        {
            _communityRepository = repository;
        }

        public async Task<CommunityDto?> Handle(
          GetCommunityByCodeQuery request,
          CancellationToken cancellationToken)
        {
            var communities = await _communityRepository.GetByCodeAsync(request.Code);

            if (communities == null)
                return null;

            return new CommunityDto
            {
                idCommunity = communities.idCommunity,
                Code = communities.Code,
                Description = communities.Description,
                Active = communities.Active
            };
        }
    }
}
