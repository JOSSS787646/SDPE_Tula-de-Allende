using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.Permission_Service.Commands.UpdatePermissions;
using SistemaDigitalizacionPolizas.Application.Services.Permission_Service.Queries.GetAllPermissions;

namespace SistemaDigitalizacionPolizas.API.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //Actualiza o agrega permisos a un rol
        [HttpPost("update")]
        public async Task<IActionResult> ActualizarPermisos(
        [FromBody] UpdatePermissionByRoleCommand command)
        {
            await _mediator.Send(command);
            return Ok("Permisos actualizados correctamente");
        }

        //Obtiene los permisos asignados a un rol
        [HttpGet("{idRol}")]
        public async Task<IActionResult> GetPermissionsByRole(int idRol)
        {
            var result = await _mediator.Send(
                new GetPermissionsByRoleQuery { IdRol = idRol }
            );

            return Ok(result);
        }
    }
}
