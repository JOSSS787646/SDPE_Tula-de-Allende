using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Queries.GetAllUser
{
    public record GetUsersPagedQuery(int Page, int PageSize)
    : IRequest<List<UserListDto>>;

}
