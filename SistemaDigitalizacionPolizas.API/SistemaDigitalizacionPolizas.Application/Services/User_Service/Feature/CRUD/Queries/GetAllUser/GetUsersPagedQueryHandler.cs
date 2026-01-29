using SistemaDigitalizacionPolizas.Application.Services.Role_Service.Queries.GetAllRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Queries.GetAllUser
{
    public class GetUsersPagedQueryHandler
    : IRequestHandler<GetUsersPagedQuery, List<UserListDto>>
    {
        private readonly IUserRepository _repository;

        public GetUsersPagedQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<UserListDto>> Handle(
            GetUsersPagedQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetUsersAsync(
                request.Page,
                request.PageSize
            );
        }
    }
}
