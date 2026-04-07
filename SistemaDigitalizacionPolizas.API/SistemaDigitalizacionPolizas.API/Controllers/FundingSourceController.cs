using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.AddFundingSource;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.UpdateFundingSource;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.UpdateStateFunding;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Queries.GetAllFundingSource;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Queries.GetByCodeFundingSource;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Gestiona las fuentes de financiamiento (crear, consultar y actualizar).
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FundingSourceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FundingSourceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crear fuente de financiamiento.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateFundingSourceCommand command)
        {
            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }

        /// <summary>
        /// Obtener todas las fuentes de financiamiento.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllFundingSourceQuery());

            return Ok(result);
        }

        /// <summary>
        /// Obtener fuente de financiamiento por código.
        /// </summary>
        [HttpGet("{code:int}")]
        public async Task<ActionResult<FundingSourceDto>> GetByCode(int code)
        {
            var result = await _mediator.Send(new GetByCodeFundingSourceQuery(code));

            if (result is null)
                return NotFound($"No existe una fuente de financiamiento con código {code}.");

            return Ok(result);
        }

        /// <summary>
        /// Actualizar fuente de financiamiento.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFundingSourceCommand command)
        {
            var fixedCommand = command with { idFundingSource = id };

            var success = await _mediator.Send(fixedCommand);

            if (!success)
                return NotFound($"No existe una fuente de financiamiento con id {id}.");

            return NoContent();
        }

        /// <summary>
        /// Cambiar estatus (activo/inactivo).
        /// </summary>
        [HttpPatch("{code:int}/status")]
        public async Task<IActionResult> ChangeStatus(int code, [FromBody] bool active)
        {
            var success = await _mediator.Send(
                new UpdateStateFundingCommand(code, active)
            );

            if (!success)
                return NotFound($"No existe una fuente de financiamiento con código {code}.");

            return NoContent();
        }
    }
}