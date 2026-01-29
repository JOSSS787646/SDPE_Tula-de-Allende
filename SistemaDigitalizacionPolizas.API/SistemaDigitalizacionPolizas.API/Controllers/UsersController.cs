using SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Command.CreatedUser;
using SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Command.UpdateStatusUser;
using SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Queries.GetAllUser;
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

        //Obtiene todos los usuarios paginados
        [HttpGet("paged")]
        public async Task<IActionResult> GetUsers(
         [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(
                new GetUsersPagedQuery(page, pageSize)
            );

            return Ok(result);
        }

        //Actualiza el estado
        [HttpPut("change-status")]
        public async Task<IActionResult> ChangeStatus(
        [FromBody] UpdateUserStatusDto dto)
        {
            var result = await _mediator.Send(
                new UpdateUserStatusCommand(dto)
            );

            if (!result)
                return NotFound("Usuario no encontrado");

            return Ok("Estado actualizado correctamente");
        }
    }
}
