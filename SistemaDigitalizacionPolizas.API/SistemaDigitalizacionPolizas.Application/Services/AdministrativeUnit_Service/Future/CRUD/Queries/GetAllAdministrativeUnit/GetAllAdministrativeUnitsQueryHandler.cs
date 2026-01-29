using SistemaDigitalizacionPolizas.Domain.Dtos.AdministrativeUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Queries.GetAllAdministrativeUnit
{
    public class GetAllAdministrativeUnitQueryHandler
    : IRequestHandler<GetAllAdministrativeUnitQuery, List<AdministrativeUnitDto>>
    {
        private readonly IAdministrativeUnit _repository;

        public GetAllAdministrativeUnitQueryHandler(IAdministrativeUnit repository)
        {
            _repository = repository;
        }

        public async Task<List<AdministrativeUnitDto>> Handle(
            GetAllAdministrativeUnitQuery request,
            CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync();

            return entities.Select(e => new AdministrativeUnitDto
            {
                IdAdministrativeUnit = e.IdAdministrativeUnit,
                Code = e.Code,
                Description = e.Description
            }).ToList();
        }
    }
}
