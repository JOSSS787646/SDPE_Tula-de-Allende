using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.CreateCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateStateCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Queries.GetAllCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Queries.GetCogByCode;
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
    public class ProyectController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ProyectController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // Crear un nuevo proyecto
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreatedProyectCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }

        // Obtener todos los proyectos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(
                new GetAllProyectQuery()
            );

            return Ok(result);
        }

        //Obtiene un proyecto por su codigo
        [HttpGet("{code:int}")]
        public async Task<ActionResult<ProyectDto>> GetByCode(int code)
        {
            var result = await _mediator.Send(new GetProyectByCodeQuery(code));

            if (result == null)
                return NotFound($"No existe un COG con código {code}");

            return Ok(result);
        }

        //Actualiza la data de un proyecto

        [HttpPut("{code:int}")]
        public async Task<IActionResult> Update(
         int code,
         [FromBody] UpdateProyectCommand command)
        {
            if (code != command.Code)
                return BadRequest("El código de la URL no coincide con el cuerpo de la solicitud.");

            var success = await _mediator.Send(command);

            if (!success)
                return NotFound($"No existe un proyecto con código {code}");

            return NoContent();
        }


       
        // --------------------------------------------------
        // Cambia el estado (activar / desactivar) de un Proyecto
        // --------------------------------------------------
        [HttpPatch("{code:int}/active")]
        public async Task<IActionResult> ChangeStatus(
            int code,
            [FromBody] bool active)
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

