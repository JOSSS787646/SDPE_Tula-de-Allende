namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Commands.DeleteRole
{
    public record DeleteRoleCommand(int IdRol) : IRequest<bool>;
}
