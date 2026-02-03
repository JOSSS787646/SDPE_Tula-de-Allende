namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Commands.CreatedRole
{
    public record CreateRoleCommand(
        string RolName,
        string Description,
        bool Active
    ) : IRequest<int>;
}
