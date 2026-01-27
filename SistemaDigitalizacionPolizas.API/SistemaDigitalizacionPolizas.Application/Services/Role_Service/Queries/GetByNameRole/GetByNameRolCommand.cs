using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Queries.GetByNameRol
{
    public record GetByNameRolCommand(string RolName)
     : IRequest<RoleDto?>;
}
