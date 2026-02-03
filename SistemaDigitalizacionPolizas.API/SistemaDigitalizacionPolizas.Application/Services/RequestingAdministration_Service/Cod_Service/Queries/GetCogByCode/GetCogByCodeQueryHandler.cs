using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Queries.GetCogByCode
{
    public class GetCogByCodeQueryHandler
        : IRequestHandler<GetCogByCodeQuery, COGDto?>
    {

        private ICogRepository _repository;
        public GetCogByCodeQueryHandler(ICogRepository repository) {

            _repository = repository;
        }

        public async Task<COGDto?> Handle(
            GetCogByCodeQuery request,
            CancellationToken cancellationToken)
        {
            var cog = await _repository.GetByCodeAsync(request.Code);
            if (cog == null)
                return null;
            return new COGDto
            {
                idCog = cog.idCog,
                Code = cog.Code,
                Description = cog.Description,
                Active = cog.Active
            };
        }

    }
}
