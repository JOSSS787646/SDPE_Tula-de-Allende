namespace SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Queries.GetAllUser
{
    public record GetUsersPagedQuery(int Page, int PageSize)
    : IRequest<List<UserListDto>>;

}
