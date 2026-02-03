namespace SistemaDigitalizacionPolizas.Application.Services.Permission_Service.Commands.UpdatePermissions
{
    public class UpdatePermissionByRoleCommand : IRequest
    {
        public int IdRol { get; set; }
        public List<int> Permisos { get; set; } = new();
    }
}
