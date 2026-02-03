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
