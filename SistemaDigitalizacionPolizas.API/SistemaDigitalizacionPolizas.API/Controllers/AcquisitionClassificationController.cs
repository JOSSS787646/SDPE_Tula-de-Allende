using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionClassification_Service.Commands.CreateAcquisitionClassification;
using SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionClassification_Service.Commands.UpdateAcquisitionClassification;
using SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionClassification_Service.Commands.UpdateStatusAcqClassification;
using SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionClassification_Service.Queries.GetAcqClassificationByCode;
using SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionClassification_Service.Queries.GetAllAcqClassification;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar clasificaciones de adquisición.
    /// Permite crear, consultar, actualizar y cambiar estatus.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AcquisitionClassificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AcquisitionClassificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crea una nueva clasificación.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] AddAcquisitionClassificationCmd command)
        {
            var id = await _mediator.Send(command);

            // Si ya existe el código
            if (id == 0)
                return Conflict("Ya existe una clasificación de adquisición con ese código.");

            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }

        /// <summary>
        /// Obtiene todas las clasificaciones.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllAcqClassificationQuery());
            return Ok(result);
        }

        /// <summary>
        /// Obtiene una clasificación por código.
        /// </summary>
        [HttpGet("{code:int}")]
        public async Task<IActionResult> GetByCode(int code)
        {
            var result = await _mediator.Send(new GetAcqClassByCodeQuery(code));

            if (result is null)
                return NotFound("Clasificación de adquisición no encontrada.");

            return Ok(result);
        }

        /// <summary>
        /// Actualiza una clasificación por Id.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAcqClassificationCmd command)
        {
            // Asegura que el ID venga desde la URL
            var fixedCommand = command with { idUpdateAcquisitionClassification = id };

            var success = await _mediator.Send(fixedCommand);

            if (!success)
                return NotFound($"No existe una clasificación con id {id}.");

            return NoContent();
        }

        /// <summary>
        /// Cambia el estatus (activo/inactivo).
        /// </summary>
        [HttpPatch("{code:int}/active")]
        public async Task<IActionResult> ChangeStatus(int code, [FromBody] bool active)
        {
            var success = await _mediator.Send(new UpdateStatusAcqClassificationCmd(code, active));

            if (!success)
                return NotFound($"No existe una clasificación con código {code}.");

            return NoContent();
        }
    }
}