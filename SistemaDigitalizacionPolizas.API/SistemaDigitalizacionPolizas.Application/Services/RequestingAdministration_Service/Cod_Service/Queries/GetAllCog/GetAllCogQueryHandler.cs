using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Queries.GetAllCog
{
    public class GetAllCogQueryHandler
    : IRequestHandler<GetAllCogQuery, List<COGDto>>
    {
        private readonly ICogRepository _repository;

        public GetAllCogQueryHandler(ICogRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<COGDto>> Handle(
            GetAllCogQuery request,
            CancellationToken cancellationToken)
        {
            var cogs = await _repository.GetAllAsync();

            return cogs.Select(r => new COGDto
            {
                idCog = r.Code,
                Code = r.Code,
                Description = r.Description,
                Active = r.Active
            }).ToList();
        }
    }

}
