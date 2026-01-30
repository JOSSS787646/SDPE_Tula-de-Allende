using MediatR;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateStateCog
{
    public class UpdateStatusCogCommandHandler
        : IRequestHandler<UpdateStatusCogCommand, bool>
    {
        private readonly ICogRepository _repository;

        public UpdateStatusCogCommandHandler(ICogRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateStatusCogCommand request,
            CancellationToken cancellationToken)
        {
            var cog = await _repository.GetByCodeAsync(request.Code);

            if (cog == null)
                return false;

            cog.Active = false;

            return await _repository.UpdateAsync(cog);
        }
    }
}
