using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Commands.CreateAcquisitionType;
using SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Commands.UpdateAcquisitionType;
using SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Commands.UpdateStatusAcquisitionType;
using SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Queries.GetAcquisitionTypeByCode;
using SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Queries.GetAllAcqusitionType;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Gestión de Tipos de Adquisición.
    /// Permite crear, consultar, actualizar y cambiar estatus.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AcquisitionTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AcquisitionTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crear tipo de adquisición.
        /// </summary>

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateAcquisitionTypeCommand command)
        {
            var id = await _mediator.Send(command);

            if (id == 0)
                return Conflict("Ya existe un tipo de adquisición con ese código.");

            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }

        /// <summary>
        /// Obtener todos los tipos de adquisición.
        /// </summary>

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllAcquisitionTypeQuery());
            return Ok(result);
        }

        /// <summary>
        /// Obtener tipo de adquisición por código.
        /// </summary>

        [HttpGet("{code:int}")]
        public async Task<IActionResult> GetByCode(int code)
        {
            var result = await _mediator.Send(new GetAcquisitionTypeByCodeQuery(code));

            if (result is null)
                return NotFound("Tipo de adquisición no encontrado.");

            return Ok(result);
        }

        /// <summary>
        /// Actualizar tipo de adquisición.
        /// </summary>

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAcquisitionTypeCommand command)
        {
            var fixedCommand = command with { idUpdateAcquisitionType = id };

            var success = await _mediator.Send(fixedCommand);

            if (!success)
                return NotFound($"No existe un tipo de adquisición con id {id}.");

            return NoContent();
        }

        /// <summary>
        /// Cambiar estatus (activo/inactivo).
        /// </summary>
 
        [HttpPatch("{code:int}/status")]
        public async Task<IActionResult> ChangeStatus(int code, [FromBody] bool active)
        {
            var success = await _mediator.Send(new UpdateStatusAcquisitionTypeCommand(code, active));

            if (!success)
                return NotFound($"No existe un tipo de adquisición con código {code}.");

            return NoContent();
        }
    }
}