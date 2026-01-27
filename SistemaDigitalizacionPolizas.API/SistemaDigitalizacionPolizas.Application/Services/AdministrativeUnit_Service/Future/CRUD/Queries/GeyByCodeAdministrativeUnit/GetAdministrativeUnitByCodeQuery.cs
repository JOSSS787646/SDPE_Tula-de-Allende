using SistemaDigitalizacionPolizas.Domain.Dtos.AdministrativeUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Queries.GeyByIdAdministrativeUnit
{
    public record GetAdministrativeUnitByCodeQuery(int Code)
  : IRequest<AdministrativeUnitDto?>;


}
