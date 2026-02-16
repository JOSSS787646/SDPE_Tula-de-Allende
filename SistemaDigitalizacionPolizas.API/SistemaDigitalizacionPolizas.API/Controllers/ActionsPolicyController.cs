using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.ActionPolicy_Service.Commands.CreateActionPolicy;
using SistemaDigitalizacionPolizas.Application.Services.ActionPolicy_Service.Commands.UpdateActionPolicy;
using SistemaDigitalizacionPolizas.Application.Services.ActionPolicy_Service.Commands.UpdateStatusActionPolicy;
using SistemaDigitalizacionPolizas.Application.Services.ActionPolicy_Service.Queries.GetActionPolicyByCode;
using SistemaDigitalizacionPolizas.Application.Services.ActionPolicy_Service.Queries.GetAllActionPolicy;


namespace SistemaDigitalizacionPolizas.API.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ActionsPolicyController : ControllerBase
    {


        private readonly IMediator _mediator;

        public ActionsPolicyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Crear una acción de política

        [HttpPost]
        public async Task<ActionResult<int>> Create(
            [FromBody] CreateActionPolicyCommand command)
        {
            var id = await _mediator.Send(command);

            if (id == 0)
                return Conflict("Ya existe una accion con ese código");

            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }


        //Obtener todas las acciones
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(
                new GetAllActionPolicyQuery()
            );

            return Ok(result);
        }

        //Obtener una accion por su codigo
        [HttpGet("{code:int}")]
        public async Task<IActionResult> GetByCode(int code)
        {
            var result = await _mediator.Send(
                new GetActionPolicyByCodeQuery(code)
            );

            if (result == null)
                return NotFound("Unidad administrativa no encontrada");

            return Ok(result);
        }

        //Actualiza por id
        // Actualiza una acción por id
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
int id,
[FromBody] UpdateActionPolicyCommand command)
        {
            // Forzamos el ID desde la URL
            var fixedCommand = command with { idActionPolicy = id };

            var success = await _mediator.Send(fixedCommand);

            if (!success)
                return NotFound($"No existe una accion con id {id}");

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
                new UpdateStatusActionPolicyCommand(code, active)
            );

            if (!success)
                return NotFound($"No existe una accion con código {code}");

            return NoContent();
        }


    }
}
