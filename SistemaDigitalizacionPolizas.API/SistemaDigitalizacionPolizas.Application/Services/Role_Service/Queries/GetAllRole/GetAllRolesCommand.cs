using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Queries.GetAllRoles
{
    public record GetAllRolesCommand()
     : IRequest<List<RoleDto>>;
}
