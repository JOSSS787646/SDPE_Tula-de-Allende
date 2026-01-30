namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Queries.GetAllRoles
{
    public record GetAllRolesCommand()
     : IRequest<List<RoleDto>>;
}
