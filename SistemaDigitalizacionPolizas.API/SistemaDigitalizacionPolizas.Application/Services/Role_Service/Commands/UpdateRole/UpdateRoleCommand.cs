using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Commands.UpdateRole
{
    public record UpdateRoleCommand(
        int IdRol,
        string RolName,
        string Description,
        bool Active
    ) : IRequest<bool>;

}
