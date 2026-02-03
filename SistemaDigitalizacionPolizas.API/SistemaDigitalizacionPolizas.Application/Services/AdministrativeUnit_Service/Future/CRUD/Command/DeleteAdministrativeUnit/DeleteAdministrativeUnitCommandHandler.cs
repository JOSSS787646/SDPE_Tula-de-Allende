namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.DeleteAdministrativeUnit
{
    public class DeleteAdministrativeUnitCommandHandler
         : IRequestHandler<DeleteAdministrativeUnitCommand, bool>
    {
        private readonly IAdministrativeUnit _repository;

        public DeleteAdministrativeUnitCommandHandler(IAdministrativeUnit repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteAdministrativeUnitCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(request.Code);
        }
    }
}
