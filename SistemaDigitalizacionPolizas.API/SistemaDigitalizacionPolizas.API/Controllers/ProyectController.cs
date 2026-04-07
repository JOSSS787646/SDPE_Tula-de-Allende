using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.CreatedProyect;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.UpdateProyect;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.UpdateStatusProyect;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Queries.GetAllProyect;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Queries.GetProyectByCode;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Gestiona los proyectos (crear, consultar y actualizar).
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProyectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProyectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crear un nuevo proyecto.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreatedProyectCommand command)
        {
            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }

        /// <summary>
        /// Obtener todos los proyectos.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllProyectQuery());

            return Ok(result);
        }

        /// <summary>
        /// Obtener proyecto por código.
        /// </summary>
        [HttpGet("{code:int}")]
        public async Task<ActionResult<ProyectDto>> GetByCode(int code)
        {
            var result = await _mediator.Send(new GetProyectByCodeQuery(code));

            if (result == null)
                return NotFound($"No existe un proyecto con código {code}");

            return Ok(result);
        }

        /// <summary>
        /// Actualizar proyecto.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProyectCommand command)
        {
            var fixedCommand = command with { idProyect = id };

            var success = await _mediator.Send(fixedCommand);

            if (!success)
                return NotFound($"No existe un proyecto con id {id}");

            return NoContent();
        }

        /// <summary>
        /// Cambiar estatus (activo/inactivo) del proyecto.
        /// </summary>
        [HttpPatch("{code:int}/active")]
        public async Task<IActionResult> ChangeStatus(int code, [FromBody] bool active)
        {
            var success = await _mediator.Send(
                new UpdateStatusProyectCommand(code, active)
            );

            if (!success)
                return NotFound($"No existe un Proyecto con código {code}");

            return NoContent();
        }
    }
}