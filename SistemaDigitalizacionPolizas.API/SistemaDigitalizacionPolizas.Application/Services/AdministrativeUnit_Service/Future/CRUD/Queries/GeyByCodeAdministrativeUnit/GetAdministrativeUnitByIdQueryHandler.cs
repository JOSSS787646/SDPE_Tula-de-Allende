using SistemaDigitalizacionPolizas.Domain.Dtos.AdministrativeUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Queries.GeyByIdAdministrativeUnit
{
    public class GetAdministrativeUnitByIdQueryHandler
    : IRequestHandler<GetAdministrativeUnitByCodeQuery, AdministrativeUnitDto?>
    {
        private readonly IAdministrativeUnit _repository;

        public GetAdministrativeUnitByIdQueryHandler(
            IAdministrativeUnit repository)
        {
            _repository = repository;
        }

        public async Task<AdministrativeUnitDto?> Handle(
            GetAdministrativeUnitByCodeQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByCodeAsync(request.Code);

            if (entity == null)
                return null;

            return new AdministrativeUnitDto
            {
                Code = entity.Code,
                Description = entity.Description
            };
        }
    }
}
