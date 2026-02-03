namespace SistemaDigitalizacionPolizas.Domain.Dtos.Permission
{

    //Devuelve lso permisosque tiene un rol (Administrador)
    public class PermissionUpdateRoleDto
    {
        public int IdRol { get; set; }
        public List<int> Permisos { get; set; } = new();

    }
}
