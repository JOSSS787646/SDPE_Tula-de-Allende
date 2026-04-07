using SistemaDigitalizacionPolizas.Domain.Dtos.Permission;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Permissions
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de permisos
    /// asociados a roles.
    ///
    /// Permite consultar y actualizar los permisos asignados a un rol
    /// dentro del sistema.
    /// </summary>
    public interface IPermissionRepository
    {
        /// <summary>
        /// Obtiene la lista de permisos asociados a un rol específico.
        /// </summary>
        Task<List<PermissionDto>> GetPermissionByRole(int idRol);

        /// <summary>
        /// Actualiza los permisos asignados a un rol.
        /// </summary>
        Task UpdatePermissionByRole(PermissionUpdateRoleDto dto);
    }
}
