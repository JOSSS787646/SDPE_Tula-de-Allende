using SistemaDigitalizacionPolizas.Domain.Dtos.Permission;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Permissions
{
    public interface IPermissionRepository
    {
        Task<List<PermissionDto>> GetPermissionByRole(int idRol);
        Task UpdatePermissionByRole(PermissionUpdateRoleDto dto);
    }
}
