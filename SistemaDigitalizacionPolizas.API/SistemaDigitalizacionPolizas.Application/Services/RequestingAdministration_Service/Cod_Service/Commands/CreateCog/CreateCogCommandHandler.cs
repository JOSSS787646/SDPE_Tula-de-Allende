using MediatR;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.CreateCog
{
    public class CreateCogCommandHandler
        : IRequestHandler<CreateCogCommand, int>
    {
        private readonly ICogRepository _repository;

        public CreateCogCommandHandler(ICogRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(
            CreateCogCommand request,
            CancellationToken cancellationToken)
        {
            var cog = new COG
            {
                Code = request.Code,
                Description = request.Description,
                Active = request.Active
            };

            var result = await _repository.AddAsync(cog);

            if (result == null)
                throw new InvalidOperationException(
                    $"Ya existe un COG con el código {request.Code}");

            return result.idCog;
        }
    }
}
