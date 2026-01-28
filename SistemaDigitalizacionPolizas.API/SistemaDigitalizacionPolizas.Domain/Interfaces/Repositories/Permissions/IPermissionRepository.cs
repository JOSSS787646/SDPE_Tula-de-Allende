using SistemaDigitalizacionPolizas.Domain.Dtos.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Permissions
{
    public interface IPermissionRepository
    {
        Task<List<PermissionDto>> GetPermissionByRole(int idRol);
        Task UpdatePermissionByRole(PermissionUpdateRoleDto dto);
    }
}
