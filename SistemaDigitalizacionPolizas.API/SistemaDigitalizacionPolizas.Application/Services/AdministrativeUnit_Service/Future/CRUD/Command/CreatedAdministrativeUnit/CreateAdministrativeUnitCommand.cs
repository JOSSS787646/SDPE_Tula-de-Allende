using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.CreatedAdministrativeUnit
{
    public record CreateAdministrativeUnitCommand(
     int Code,
     string Description
 ) : IRequest<int>;
}
