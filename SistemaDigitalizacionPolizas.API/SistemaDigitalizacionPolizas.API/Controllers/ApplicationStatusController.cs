using Microsoft.AspNetCore.Authorization;
using MediatR;
using SistemaDigitalizacionPolizas.Application.Services.AppliccationStatus_Service.Commands.CreateApplicationStatus;
using SistemaDigitalizacionPolizas.Application.Services.AppliccationStatus_Service.Commands.UpdateApplicationStatus;
using SistemaDigitalizacionPolizas.Application.Services.AppliccationStatus_Service.Queries.GetAllApplicationStatus;
using SistemaDigitalizacionPolizas.Application.Services.AppliccationStatus_Service.Queries.GetApplicationStatusByCode;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Gestiona los estados de solicitud (crear, consultar y actualizar).
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationStatusController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationStatusController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crear un estado de solicitud.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateApplicationStatusCommand command)
        {
            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }

        /// <summary>
        /// Obtener estado por código.
        /// </summary>
        [HttpGet("by-code/{code:int}")]
        public async Task<IActionResult> GetByCode(int code)
        {
            var result = await _mediator.Send(new GetApplicationStatusByCodeQuery(code));

            if (result is null)
                return NotFound("Estado no encontrado.");

            return Ok(result);
        }

        /// <summary>
        /// Obtener todos los estados.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllApplicationStatusQuery());

            return Ok(result);
        }

        /// <summary>
        /// Actualizar estado por id.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateApplicationStatusCommand command)
        {
            var fixedCommand = command with { IdApplicationStatus = id };

            var success = await _mediator.Send(fixedCommand);

            if (!success)
                return NotFound($"No existe un estado con id {id}.");

            return NoContent();
        }

        /// <summary>
        /// Cambiar estatus (activo/inactivo).
        /// </summary>
        [HttpPatch("code/{code:int}/status")]
        public async Task<IActionResult> UpdateStateByCode(int code, [FromBody] bool active)
        {
            var success = await _mediator.Send(new UpdateApplicationStatusStateCommand(code, active));

            if (!success)
                return NotFound($"No existe un estado con código {code}.");

            return NoContent();
        }
    }
}