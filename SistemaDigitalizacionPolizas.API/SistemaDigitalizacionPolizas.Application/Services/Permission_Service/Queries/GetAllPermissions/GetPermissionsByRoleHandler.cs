using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Permissions;

namespace SistemaDigitalizacionPolizas.Application.Services.Permission_Service.Queries.GetAllPermissions
{
    public class GetPermissionsByRoleHandler
    : IRequestHandler<GetPermissionsByRoleQuery, List<PermissionDto>>
    {
        private readonly IPermissionRepository _repository;

        public GetPermissionsByRoleHandler(IPermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PermissionDto>> Handle(
        GetPermissionsByRoleQuery request,
        CancellationToken cancellationToken)
        {
            var result = await _repository.GetPermissionByRole(request.IdRol);


            return result;
        }
    }

}
