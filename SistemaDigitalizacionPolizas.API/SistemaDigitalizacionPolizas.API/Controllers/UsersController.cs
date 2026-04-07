using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Command.CreatedUser;
using SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Command.UpdateStatusUser;
using SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Command.UpdateUserData;
using SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Queries.GetAllUser;
using SistemaDigitalizacionPolizas.Domain.Dtos.User;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Gestiona usuarios del sistema (crear, consultar y actualizar).
    /// </summary>
    //[Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crear un usuario.
        /// </summary>
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

        /// <summary>
        /// Obtener usuarios paginados.
        /// </summary>
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

        /// <summary>
        /// Actualizar estado de usuario.
        /// </summary>
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

        /// <summary>
        /// Actualizar datos de usuario.
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserData(
          [FromBody] UpdateUserDataCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound("Usuario no enconttrado");

            return Ok("Usuario actualizado correctamente");
        }
    }
}