namespace SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Command.UpdateStatusUser
{
    public record UpdateUserStatusCommand(UpdateUserStatusDto Data)
    : IRequest<bool>;
}
