using MediatR;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.UpdateStatusProyect;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateStateCog
{
    public class UpdateStatusCogCommandHandler
        : IRequestHandler<UpdateStatusCogCommand, bool>
    {
        private readonly ICogRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public UpdateStatusCogCommandHandler(ICogRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUser = currentUserService;
        }

        public async Task<bool> Handle(
       UpdateStatusCogCommand request,
       CancellationToken cancellationToken)
        {
            var proyect = await _repository.GetByCodeAsync(request.Code);

            if (proyect == null)
                return false;


            proyect.Active = request.Active;

            // Auditoría
            proyect.UpdatedBy = _currentUser.UserId;
            proyect.UpdatedAt = DateTime.Now;

            return await _repository.UpdateAsync(proyect);
        }
    }
}
