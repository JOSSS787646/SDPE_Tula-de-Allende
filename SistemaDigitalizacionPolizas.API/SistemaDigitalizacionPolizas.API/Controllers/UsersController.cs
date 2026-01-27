using SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Command.CreatedUser;
using SistemaDigitalizacionPolizas.Domain.Dtos.User;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //Permite crear un usuario

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var command = new CreateUserCommand(
                request.Email,
                request.Password,
                request.IdAdministrativeUnit,
                request.IdRole
            );

            var userId = await _mediator.Send(command);

            return CreatedAtAction(nameof(CreateUser), new
            {
                id = userId
            });
        }
    }
}
