using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Commands.DeleteRole
{
    public record DeleteRoleCommand(int IdRol) : IRequest<bool>;
}
