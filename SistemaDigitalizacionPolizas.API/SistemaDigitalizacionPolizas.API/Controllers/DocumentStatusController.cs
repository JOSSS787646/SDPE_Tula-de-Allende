using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Commands.CreateDocumentStatus;
using SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Commands.DocumentStatusActive;
using SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Commands.UpdateDocumentStatus;
using SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Queries.GetAllDocumentStatus;
using SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Queries.GetDocumentStatusByCode;

namespace SistemaDigitalizacionPolizas.API.Controllers
{

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

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateDocumentStatusCommand command)
        {
            var id = await _mediator.Send(command);

            if (id == 0)
                return Conflict("Ya existe un estado de documento con ese código.");

            return Ok(new { id });
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllDocumentStatusQuery());
            return Ok(result);
        }

        [HttpGet("code/{code:int}")]
        public async Task<IActionResult> GetByCode([FromRoute] int code)
        {
            var result = await _mediator.Send(new GetDocumentStatusByCodeQuery(code));

            if (result == null)
                return NotFound("No se encontró un estado de documento con ese código.");

            return Ok(result);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] UpdateDocumentStatusCommand command)
        {
            // 🔐 Forzamos el Id desde la URL, ignorando el del body
            var fixedCommand = command with { IdDocumentStatus = id };

            var updated = await _mediator.Send(fixedCommand);

            if (!updated)
                return NotFound("No se encontró el estado de documento con ese id.");

            return Ok("Estado de documento actualizado correctamente.");
        }

        [HttpPatch("{code:int}/active")]
        public async Task<IActionResult> PatchActive(
         [FromRoute] int code,
         [FromBody] bool active)
        {
            var command = new DocumentStatusActiveCommand(code, active);

            var updated = await _mediator.Send(command);

            if (!updated)
                return NotFound("No se encontró el estado de documento con ese código.");

            return Ok("Estado de documento actualizado (Active) correctamente.");
        }
    }
}
