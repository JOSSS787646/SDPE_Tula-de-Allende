

namespace SistemaDigitalizacionPolizas.Domain.Entities.Permissions_Entities
{
    public class Permission
    {
        public int IdPermission { get; set; }
        public string PermissionName { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }

        public ICollection<PermissionRole> PermissionRoles { get; set; }
    }
}
