using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Commands.UpdateStatusProg;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Community;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Community_Service.Commands.UpdateStatusCommunity
{
    public class UpdateStatusCommunityCommandHandler
        : IRequestHandler<UpdateStatusCommunityCommand, bool>
    {

        private readonly ICommunityRepository _communityRepository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateStatusCommunityCommandHandler(ICommunityRepository repository, ICurrentUserService currentUserService)
        {
            _communityRepository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(
 UpdateStatusCommunityCommand request,
 CancellationToken cancellationToken)
        {
            var prog = await _communityRepository.GetByCodeAsync(request.Code);

            if (prog == null)
                return false;
            prog.Active = request.Active;
            prog.UpdatedBy = _currentUserService.UserId;
            prog.UpdatedAt = DateTime.UtcNow;

            return await _communityRepository.UpdateAsync(prog);
        }
    }
}
