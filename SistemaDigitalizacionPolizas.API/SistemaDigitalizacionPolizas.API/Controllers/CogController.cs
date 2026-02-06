using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.CreateCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateStateCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Queries.GetAllCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Queries.GetCogByCode;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.UpdateStatusProyect;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
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


        //Obtener todos los cogs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(
                new GetAllCogQuery()
            );

            return Ok(result);
        }

        // Obtiene un COG por Code
        // --------------------------------------------------
        [HttpGet("{code:int}")]
        public async Task<ActionResult<COGDto>> GetByCode(int code)
        {
            var result = await _mediator.Send(new GetCogByCodeQuery(code));

            if (result == null)
                return NotFound($"No existe un COG con código {code}");

            return Ok(result);
        }

        // Crear un nuevo COG
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateCogCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }

        //Actualiza un COG existente

        [HttpPut("{code:int}")]
        public async Task<IActionResult> Update(
         int code,
         [FromBody] UpdateCogCommand command)
        {
            if (code != command.Code)
                return BadRequest("El código de la URL no coincide con el cuerpo de la solicitud.");

            var success = await _mediator.Send(command);

            if (!success)
                return NotFound($"No existe un COG con código {code}");

            return NoContent();
        }


        // Desactiva (soft delete) un COG
        // --------------------------------------------------
        [HttpPatch("{code:int}/active")]
        public async Task<IActionResult> ChangeStatus(
        int code,
        [FromBody] bool active)
        {
            var success = await _mediator.Send(
                new UpdateStatusCogCommand(code, active)
            );

            if (!success)
                return NotFound($"No existe un Proyecto con código {code}");

            return NoContent();
        }


    }
}
