using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Permission_Service.Commands.UpdatePermissions
{
    public class UpdatePermissionByRoleCommand : IRequest
    {
        public int IdRol { get; set; }
        public List<int> Permisos { get; set; } = new();
    }
}
