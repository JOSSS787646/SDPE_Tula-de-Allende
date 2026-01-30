namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Queries.GetByIdRole
{
    public record GetByIdRoleCommand(int IdRol)
     : IRequest<RoleDto?>;
}
