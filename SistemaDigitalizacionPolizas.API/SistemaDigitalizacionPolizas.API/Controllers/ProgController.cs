using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Commands.CreateProg;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Commands.UpdateProg;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Commands.UpdateStatusProg;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Queries.CreateQuery;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Queries.GetByCodeProg;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.CreatedProyect;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.UpdateProyect;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.UpdateStatusProyect;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Queries.GetAllProyect;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Queries.GetProyectByCode;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProgController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProgController(IMediator mediator)
        {
            _mediator = mediator;
        }



        // Crear un nuevo prog
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateProgCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }


        // Obtener todos los prog
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(
                new GetAllProgQuery()
            );

            return Ok(result);
        }


        //Obtiene un proyecto por su codigo
        [HttpGet("{code:int}")]
        public async Task<ActionResult<ProgDto>> GetByCode(int code)
        {
            var result = await _mediator.Send(new GetByCodeProgQuery(code));

            if (result == null)
                return NotFound($"No existe un COG con código {code}");

            return Ok(result);
        }

        //Actualiza la data de un prog

        [HttpPut("{code:int}")]
        public async Task<IActionResult> Update(
         int code,
         [FromBody] UpdateProgCommand command)
        {
            if (code != command.Code)
                return BadRequest("El código de la URL no coincide con el cuerpo de la solicitud.");

            var success = await _mediator.Send(command);

            if (!success)
                return NotFound($"No existe un prog con código {code}");

            return NoContent();
        }


        // --------------------------------------------------
        // Cambia el estado (activar / desactivar) de un Prog
        // --------------------------------------------------
        [HttpPatch("{code:int}/active")]
        public async Task<IActionResult> ChangeStatus(
            int code,
            [FromBody] bool active)
        {
            var success = await _mediator.Send(
                new UpdateStatusProgCommand(code, active)
            );

            if (!success)
                return NotFound($"No existe un Prog con código {code}");

            return NoContent();
        }

    }
}
