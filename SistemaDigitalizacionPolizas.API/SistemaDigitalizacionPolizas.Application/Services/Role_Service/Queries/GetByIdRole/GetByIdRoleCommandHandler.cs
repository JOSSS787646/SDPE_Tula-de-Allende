using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Roles;

namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Queries.GetByIdRole
{
    public class GetByIdRoleCommandHandler
         : IRequestHandler<GetByIdRoleCommand, RoleDto?>
    {
        private readonly IRoleRepository _repository;

        public GetByIdRoleCommandHandler(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<RoleDto?> Handle(
          GetByIdRoleCommand request,
          CancellationToken cancellationToken)
        {
            var role = await _repository.GetByIdAsync(request.IdRol);

            if (role == null)
                return null;

            return new RoleDto
            {
                IdRol = role.IdRol,
                RolName = role.RolName,
                Description = role.Description,
                active = role.Asset
            };
        }
    }
}
