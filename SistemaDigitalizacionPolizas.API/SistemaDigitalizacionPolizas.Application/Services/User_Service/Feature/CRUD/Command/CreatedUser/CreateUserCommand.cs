namespace SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Command.CreatedUser
{
    public record CreateUserCommand(
       string Email,
       string Password,
       int IdAdministrativeUnit,
       int IdRole
   ) : IRequest<int>;

}
