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
    /// Controlador para la gestión del catálogo de Clasificación de Adquisiciones.
    /// Permite crear, consultar, actualizar y activar/desactivar clasificaciones.
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
        /// Crea una nueva Clasificación de Adquisición.
        /// </summary>
        /// <param name="command">Datos de la clasificación a crear.</param>
        /// <returns>Id del registro creado.</returns>
        /// <response code="201">Clasificación creada correctamente.</response>
        /// <response code="409">Ya existe una clasificación con el mismo código.</response>
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] AddAcquisitionClassificationCmd command)
        {
            var id = await _mediator.Send(command);

            if (id == 0)
                return Conflict("Ya existe una clasificación de adquisición con ese código.");

            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }

        /// <summary>
        /// Obtiene todas las Clasificaciones de Adquisiciones.
        /// </summary>
        /// <returns>Listado de clasificaciones.</returns>
        /// <response code="200">Listado obtenido correctamente.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllAcqClassificationQuery());
            return Ok(result);
        }

        /// <summary>
        /// Obtiene una Clasificación de Adquisición por su código.
        /// </summary>
        /// <param name="code">Código de la clasificación.</param>
        /// <returns>Clasificación encontrada.</returns>
        /// <response code="200">Clasificación encontrada.</response>
        /// <response code="404">No se encontró la clasificación.</response>
        [HttpGet("{code:int}")]
        public async Task<IActionResult> GetByCode(int code)
        {
            var result = await _mediator.Send(new GetAcqClassByCodeQuery(code));

            if (result is null)
                return NotFound("Clasificación de adquisición no encontrada.");

            return Ok(result);
        }

        /// <summary>
        /// Actualiza la información de una Clasificación de Adquisición por su Id.
        /// </summary>
        /// <param name="id">Id de la clasificación.</param>
        /// <param name="command">Datos actualizados.</param>
        /// <response code="204">Clasificación actualizada correctamente.</response>
        /// <response code="404">No se encontró la clasificación.</response>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAcqClassificationCmd command)
        {
            // Forzamos el ID desde la URL para evitar inconsistencias
            var fixedCommand = command with { idUpdateAcquisitionClassification = id };

            var success = await _mediator.Send(fixedCommand);

            if (!success)
                return NotFound($"No existe una clasificación de adquisición con id {id}.");

            return NoContent();
        }

        /// <summary>
        /// Activa o desactiva (soft delete) una Clasificación de Adquisición por su código.
        /// </summary>
        /// <param name="code">Código de la clasificación.</param>
        /// <param name="active">Estado a asignar (true = activo, false = inactivo).</param>
        /// <response code="204">Estatus actualizado correctamente.</response>
        /// <response code="404">No se encontró la clasificación.</response>
        [HttpPatch("{code:int}/active")]
        public async Task<IActionResult> ChangeStatus(int code, [FromBody] bool active)
        {
            var success = await _mediator.Send(new UpdateStatusAcqClassificationCmd(code, active));

            if (!success)
                return NotFound($"No existe una clasificación de adquisición con código {code}.");

            return NoContent();
        }
    }
}
