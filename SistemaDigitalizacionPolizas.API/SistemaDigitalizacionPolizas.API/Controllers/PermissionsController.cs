using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.Permission_Service.Commands.UpdatePermissions;
using SistemaDigitalizacionPolizas.Application.Services.Permission_Service.Queries.GetAllPermissions;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Gestiona los permisos asignados a los roles del sistema.
    /// </summary>
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Actualizar o agregar permisos a un rol.
        /// </summary>
        [HttpPost("update")]
        public async Task<IActionResult> ActualizarPermisos(
            [FromBody] UpdatePermissionByRoleCommand command)
        {
            await _mediator.Send(command);

            return Ok("Permisos actualizados correctamente");
        }

        /// <summary>
        /// Obtener permisos asignados a un rol.
        /// </summary>
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