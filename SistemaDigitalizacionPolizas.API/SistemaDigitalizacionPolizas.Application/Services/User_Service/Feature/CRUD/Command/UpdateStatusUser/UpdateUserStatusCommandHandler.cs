namespace SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Command.UpdateStatusUser
{
    public class UpdateUserStatusCommandHandler
    : IRequestHandler<UpdateUserStatusCommand, bool>
    {
        private readonly IUserRepository _repository;

        public UpdateUserStatusCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateUserStatusCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.UpdateStatusAsync(
                request.Data.IdUser,
                request.Data.Asset
            );
        }
    }
}
