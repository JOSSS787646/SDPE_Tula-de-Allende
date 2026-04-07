using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.Role_Service.Commands.CreatedRole;
using SistemaDigitalizacionPolizas.Application.Services.Role_Service.Commands.DeleteRole;
using SistemaDigitalizacionPolizas.Application.Services.Role_Service.Commands.UpdateRole;
using SistemaDigitalizacionPolizas.Application.Services.Role_Service.Queries.GetAllRoles;
using SistemaDigitalizacionPolizas.Application.Services.Role_Service.Queries.GetByIdRole;
using SistemaDigitalizacionPolizas.Application.Services.Role_Service.Queries.GetByNameRol;

namespace SistemaDigitalizacionPolizas.API.Controllers
{

    /// <summary>
    /// Gestiona roles del sistema (crear, consultar, actualizar y eliminar).
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {

        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crear un rol.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleCommand command)
        {
            var id = await _mediator.Send(command);

            return Ok(new
            {
                message = "Rol creado correctamente",
                id
            });
        }

        /// <summary>
        /// Obtener rol por nombre.
        /// </summary>
        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _mediator.Send(
                new GetByNameRolCommand(name)
            );

            if (result == null)
                return NotFound("Rol no encontrado");

            return Ok(result);
        }

        /// <summary>
        /// Obtener rol por id.
        /// </summary>
        [HttpGet("{idRol}")]
        public async Task<IActionResult> GetById(int idRol)
        {
            var result = await _mediator.Send(
                new GetByIdRoleCommand(idRol)
            );

            if (result == null)
                return NotFound("Rol no encontrado");

            return Ok(result);
        }

        /// <summary>
        /// Obtener todos los roles.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(
                new GetAllRolesCommand()
            );

            return Ok(result);
        }

        /// <summary>
        /// Actualizar un rol.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRoleCommand command)
        {
            var updated = await _mediator.Send(command);

            if (!updated)
                return NotFound("Rol no encontrado");

            return Ok("Rol actualizado correctamente");
        }

        /// <summary>
        /// Eliminar (desactivar) un rol.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(
                new DeleteRoleCommand(id)
            );

            if (!result)
                return NotFound("Rol no encontrado");

            return Ok("Rol desactivado correctamente");
        }

    }
}