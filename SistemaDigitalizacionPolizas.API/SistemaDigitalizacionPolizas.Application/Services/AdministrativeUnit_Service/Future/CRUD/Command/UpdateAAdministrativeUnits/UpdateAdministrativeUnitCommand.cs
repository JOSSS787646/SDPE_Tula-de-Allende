using SistemaDigitalizacionPolizas.Domain.Dtos.AdministrativeUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.UpdateAAdministrativeUnits
{
    public record UpdateAdministrativeUnitCommand(
     int code,
     AdministrativeUnitDto Unit
 ) : IRequest<bool>;

}
