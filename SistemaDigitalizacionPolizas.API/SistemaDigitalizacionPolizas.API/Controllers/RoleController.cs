using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.Role_Service.Commands.CreatedRole;
using SistemaDigitalizacionPolizas.Application.Services.Role_Service.Commands.DeleteRole;
using SistemaDigitalizacionPolizas.Application.Services.Role_Service.Commands.UpdateRole;
using SistemaDigitalizacionPolizas.Application.Services.Role_Service.Queries.GetAllRoles;
using SistemaDigitalizacionPolizas.Application.Services.Role_Service.Queries.GetByIdRole;
using SistemaDigitalizacionPolizas.Application.Services.Role_Service.Queries.GetByNameRol;

namespace SistemaDigitalizacionPolizas.API.Controllers
{

   // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {

        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //Edpoint para crear un rol
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


        //Endpoint que permite buscar por nombre de rol
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


        //Endpoint que permite buscar por id de rol
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
        //Obtener todos los roles
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(
                new GetAllRolesCommand()
            );

            return Ok(result);
        }

        //Actulizar un rol por id
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRoleCommand command)
        {
            var updated = await _mediator.Send(command);

            if (!updated)
                return NotFound("Rol no encontrado");

            return Ok("Rol actualizado correctamente");
        }

        //Eliminar un rol por id

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
