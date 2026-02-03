using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Roles;

namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Commands.UpdateRole
{
    public class UpdateRoleCommandHandler
       : IRequestHandler<UpdateRoleCommand, bool>
    {
        private readonly IRoleRepository _repository;

        public UpdateRoleCommandHandler(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateRoleCommand request,
            CancellationToken cancellationToken)
        {
            var role = new Role
            {
                IdRol = request.IdRol,
                RolName = request.RolName,
                Description = request.Description,
                Asset = request.Active
            };

            return await _repository.UpdateAsync(role);
        }
    }
}
