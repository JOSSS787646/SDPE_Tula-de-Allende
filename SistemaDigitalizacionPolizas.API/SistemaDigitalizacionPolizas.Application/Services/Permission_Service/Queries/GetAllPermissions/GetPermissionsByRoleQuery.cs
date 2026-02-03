namespace SistemaDigitalizacionPolizas.Application.Services.Permission_Service.Queries.GetAllPermissions
{
    public class GetPermissionsByRoleQuery
    : IRequest<List<PermissionDto>>
    {
        public int IdRol { get; set; }
    }

}
