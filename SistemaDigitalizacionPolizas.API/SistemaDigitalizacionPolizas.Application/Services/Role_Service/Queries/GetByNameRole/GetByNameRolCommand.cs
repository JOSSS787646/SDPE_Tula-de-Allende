namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Queries.GetByNameRol
{
    public record GetByNameRolCommand(string RolName)
     : IRequest<RoleDto?>;
}
