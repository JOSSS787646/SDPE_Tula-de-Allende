using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Queries.GetByNameRol
{
    public class GetByNameRolCommandHandler
        : IRequestHandler<GetByNameRolCommand, RoleDto?>
    {
        private readonly IRoleRepository _repository;

        public GetByNameRolCommandHandler(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<RoleDto?> Handle(
            GetByNameRolCommand request,
            CancellationToken cancellationToken)
        {
            var role = await _repository.GetByNameAsync(request.RolName);

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
