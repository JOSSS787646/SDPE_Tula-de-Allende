using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.CreateCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateStateCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Queries.GetAllCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Queries.GetCogByCode;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Gestiona los COG (crear, consultar y actualizar).
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtener todos los COG.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllCogQuery());

            return Ok(result);
        }

        /// <summary>
        /// Obtener COG por código.
        /// </summary>
        [HttpGet("{code:int}")]
        public async Task<ActionResult<COGDto>> GetByCode(int code)
        {
            var result = await _mediator.Send(new GetCogByCodeQuery(code));

            if (result is null)
                return NotFound($"No existe un COG con código {code}.");

            return Ok(result);
        }

        /// <summary>
        /// Crear COG.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateCogCommand command)
        {
            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }

        /// <summary>
        /// Actualizar COG.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCogCommand command)
        {
            var fixedCommand = command with { idCog = id };

            var success = await _mediator.Send(fixedCommand);

            if (!success)
                return NotFound($"No existe un COG con id {id}.");

            return NoContent();
        }

        /// <summary>
        /// Cambiar estatus (activo/inactivo).
        /// </summary>
        [HttpPatch("{code:int}/status")]
        public async Task<IActionResult> ChangeStatus(int code, [FromBody] bool active)
        {
            var success = await _mediator.Send(new UpdateStatusCogCommand(code, active));

            if (!success)
                return NotFound($"No existe un COG con código {code}.");

            return NoContent();
        }
    }
}