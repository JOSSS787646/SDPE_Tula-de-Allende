
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Communitys;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;


namespace SistemaDigitalizacionPolizas.Application.Services.Community_Service.Commands.UpdateCommunity
{
    public class UpdateComunnityCommandHandler
        : IRequestHandler<UpdateCommunityCommand, bool>
    {

        private readonly ICommunityRepository _communityRepository;
        private readonly ICurrentUserService _currentUserService;


        public UpdateComunnityCommandHandler(ICommunityRepository repository, ICurrentUserService currentUserService)
        {
            _communityRepository = repository;
            _currentUserService = currentUserService;
        }


        public async Task<bool> Handle(
    UpdateCommunityCommand request,
    CancellationToken cancellationToken)
        {

            var community = await _communityRepository.GetByIdAsync(request.idCommunity);

            if (community == null)
                return false;


            community.Code = request.Code;
            community.Description = request.Description;
            community.Active = request.Active;
            community.UpdatedBy = _currentUserService.UserId;
            community.UpdatedAt = DateTime.Now;

            return await _communityRepository.UpdateAsync(community);
        }

    }
}
