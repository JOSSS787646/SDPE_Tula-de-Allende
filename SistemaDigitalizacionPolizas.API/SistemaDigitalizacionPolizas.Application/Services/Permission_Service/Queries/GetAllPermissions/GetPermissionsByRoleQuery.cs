using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Permission_Service.Queries.GetAllPermissions
{
    public class GetPermissionsByRoleQuery
    : IRequest<List<PermissionDto>>
    {
        public int IdRol { get; set; }
    }

}
