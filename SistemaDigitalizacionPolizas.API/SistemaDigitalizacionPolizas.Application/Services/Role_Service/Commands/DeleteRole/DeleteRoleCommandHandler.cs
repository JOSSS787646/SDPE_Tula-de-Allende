using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Roles;

namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Commands.DeleteRole
{
    public class DeleteRoleCommandHandler
       : IRequestHandler<DeleteRoleCommand, bool>
    {
        private readonly IRoleRepository _repository;

        public DeleteRoleCommandHandler(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteRoleCommand request,
            CancellationToken cancellationToken)
        {
            var role = await _repository.GetByIdAsync(request.IdRol);

            if (role == null)
                return false;

            role.Asset = false;

            return await _repository.UpdateAsync(role);
        }
    }
}
