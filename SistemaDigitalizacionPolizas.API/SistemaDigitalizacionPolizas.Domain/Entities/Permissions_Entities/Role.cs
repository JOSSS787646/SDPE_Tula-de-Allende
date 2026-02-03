public class Role
{
    public int IdRol { get; set; }
    public string RolName { get; set; }
    public string Description { get; set; }
    public bool Asset { get; set; }

    // Relaciones
    public ICollection<User> Users { get; set; }
    public ICollection<PermissionRole> PermissionRoles { get; set; }
}