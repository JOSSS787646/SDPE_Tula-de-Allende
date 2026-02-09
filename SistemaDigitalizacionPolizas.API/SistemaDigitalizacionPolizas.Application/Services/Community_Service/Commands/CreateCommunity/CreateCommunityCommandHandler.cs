using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Commands.CreateProg;
using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Communitys;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Community_Service.Commands.CreateCommunity
{
    public class CreateCommunityCommandHandler
        : IRequestHandler<CreateCommunityCommand, int>
    {

        private readonly ICommunityRepository _communityRepository;
        private readonly ICurrentUserService _currentUserService;


        public CreateCommunityCommandHandler(ICommunityRepository repository, ICurrentUserService currentUserService)
        {
            _communityRepository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(
          CreateCommunityCommand request,
          CancellationToken cancellationToken)
        {
            var community = new Community
            {
                Code = request.Code,
                Description = request.Description,
                Active = true,
                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTime.Now
            };

            var result = await _communityRepository.AddAsync(community);

            if (result == null)
                throw new InvalidOperationException(
                    $"Ya existe un Proyecto con el código {request.Code}");

            return result.idCommunity;
        }
    }
}
