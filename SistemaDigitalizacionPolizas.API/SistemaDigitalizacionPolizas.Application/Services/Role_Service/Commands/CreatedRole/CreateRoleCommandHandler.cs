using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Roles;

namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Commands.CreatedRole
{
    public class CreateRoleCommandHandler
        : IRequestHandler<CreateRoleCommand, int>
    {
        private readonly IRoleRepository _repository;

        public CreateRoleCommandHandler(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(
            CreateRoleCommand request,
            CancellationToken cancellationToken)
        {
            var role = new Role
            {
                RolName = request.RolName,
                Description = request.Description,
                Asset = request.Active
            };

            await _repository.AddAsync(role);

            return role.IdRol;
        }
    }
}
