using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Commands.CreatedRole
{
    public record CreateRoleCommand(
        string RolName,
        string Description,
        bool Active
    ) : IRequest<int>;
}
