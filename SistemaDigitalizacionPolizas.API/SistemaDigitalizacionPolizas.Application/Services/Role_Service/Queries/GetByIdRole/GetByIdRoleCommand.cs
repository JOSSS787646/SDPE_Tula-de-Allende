using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Queries.GetByIdRole
{
    public record GetByIdRoleCommand(int IdRol)
     : IRequest<RoleDto?>;
}
