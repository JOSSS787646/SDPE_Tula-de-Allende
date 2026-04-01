using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Commands.CreateDocumentStatus;
using SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Commands.DocumentStatusActive;
using SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Commands.UpdateDocumentStatus;
using SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Queries.GetAllDocumentStatus;
using SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Queries.GetDocumentStatusByCode;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Gestiona los estados de documento (crear, consultar y actualizar).
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentStatusController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentStatusController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crear estado de documento.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateDocumentStatusCommand command)
        {
            var id = await _mediator.Send(command);

            if (id == 0)
                return Conflict("Ya existe un estado de documento con ese código.");

            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }

        /// <summary>
        /// Obtener todos los estados de documento.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllDocumentStatusQuery());

            return Ok(result);
        }

        /// <summary>
        /// Obtener estado de documento por código.
        /// </summary>
        [HttpGet("code/{code:int}")]
        public async Task<IActionResult> GetByCode(int code)
        {
            var result = await _mediator.Send(new GetDocumentStatusByCodeQuery(code));

            if (result is null)
                return NotFound("No existe un estado de documento con ese código.");

            return Ok(result);
        }

        /// <summary>
        /// Actualizar estado de documento.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDocumentStatusCommand command)
        {
            var fixedCommand = command with { IdDocumentStatus = id };

            var success = await _mediator.Send(fixedCommand);

            if (!success)
                return NotFound($"No existe un estado de documento con id {id}.");

            return NoContent();
        }

        /// <summary>
        /// Cambiar estatus (activo/inactivo).
        /// </summary>
        [HttpPatch("{code:int}/status")]
        public async Task<IActionResult> ChangeStatus(int code, [FromBody] bool active)
        {
            var success = await _mediator.Send(
                new DocumentStatusActiveCommand(code, active)
            );

            if (!success)
                return NotFound($"No existe un estado de documento con código {code}.");

            return NoContent();
        }
    }
}