using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Queries.GetAllRoles
{
    public class GetAllRolesCommandHandler
        : IRequestHandler<GetAllRolesCommand, List<RoleDto>>
    {
        private readonly IRoleRepository _repository;

        public GetAllRolesCommandHandler(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RoleDto>> Handle(
            GetAllRolesCommand request,
            CancellationToken cancellationToken)
        {
            var roles = await _repository.GetAllAsync();

            return roles.Select(r => new RoleDto
            {
                IdRol = r.IdRol,
                RolName = r.RolName,
                Description = r.Description,
                active = r.Asset
            }).ToList();
        }
    }
}
