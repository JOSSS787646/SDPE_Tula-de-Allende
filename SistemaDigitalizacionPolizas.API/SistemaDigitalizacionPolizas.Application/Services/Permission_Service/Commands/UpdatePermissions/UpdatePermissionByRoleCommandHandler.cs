using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Permission_Service.Commands.UpdatePermissions
{
    public class UpdatePermissionByRoleCommandHandler
    : IRequestHandler<UpdatePermissionByRoleCommand>
    {
        private readonly IPermissionRepository _repository;

        public UpdatePermissionByRoleCommandHandler(IPermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            UpdatePermissionByRoleCommand request,
            CancellationToken cancellationToken)
        {
            await _repository.UpdatePermissionByRole(
                new PermissionUpdateRoleDto
                {
                    IdRol = request.IdRol,
                    Permisos = request.Permisos
                });

            return Unit.Value;
        }
    }

}
