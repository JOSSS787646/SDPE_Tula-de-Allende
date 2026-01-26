



namespace SistemaDigitalizacionPolizas.Domain.Entities.Permissions_Entities
{
    public class PermissionRole
    {
        public int IdPermissionRole { get; set; }
        public int IdRole { get; set; }
        public int IdPermission { get; set; }


        //Relacion a la tabla Permisos
        public Role Role { get; set; }
        public Permission Permission { get; set; }

    }
}
