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
    /// Controlador para la gestión del catálogo de Tipos de Adquisición.
    /// Permite crear, consultar, actualizar y activar/desactivar tipos de adquisición.
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
        /// Crea un nuevo Tipo de Adquisición.
        /// </summary>
        /// <param name="command">Datos del tipo de adquisición a crear.</param>
        /// <returns>Id del registro creado.</returns>
        /// <response code="201">Tipo de adquisición creado correctamente.</response>
        /// <response code="409">Ya existe un tipo de adquisición con el mismo código.</response>
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateAcquisitionTypeCommand command)
        {
            var id = await _mediator.Send(command);

            if (id == 0)
                return Conflict("Ya existe un tipo de adquisición con ese código.");

            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }

        /// <summary>
        /// Obtiene todos los Tipos de Adquisición.
        /// </summary>
        /// <returns>Listado de tipos de adquisición.</returns>
        /// <response code="200">Listado obtenido correctamente.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllAcquisitionTypeQuery());
            return Ok(result);
        }

        /// <summary>
        /// Obtiene un Tipo de Adquisición por su código.
        /// </summary>
        /// <param name="code">Código del tipo de adquisición.</param>
        /// <returns>Tipo de adquisición encontrado.</returns>
        /// <response code="200">Tipo de adquisición encontrado.</response>
        /// <response code="404">No se encontró el tipo de adquisición.</response>
        [HttpGet("{code:int}")]
        public async Task<IActionResult> GetByCode(int code)
        {
            var result = await _mediator.Send(new GetAcquisitionTypeByCodeQuery(code));

            if (result is null)
                return NotFound("Tipo de adquisición no encontrado.");

            return Ok(result);
        }

        /// <summary>
        /// Actualiza la información de un Tipo de Adquisición por su Id.
        /// </summary>
        /// <param name="id">Id del tipo de adquisición.</param>
        /// <param name="command">Datos actualizados.</param>
        /// <response code="204">Tipo de adquisición actualizado correctamente.</response>
        /// <response code="404">No se encontró el tipo de adquisición.</response>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAcquisitionTypeCommand command)
        {
            // Forzamos el ID desde la URL para evitar inconsistencias
            var fixedCommand = command with { idUpdateAcquisitionType = id };

            var success = await _mediator.Send(fixedCommand);

            if (!success)
                return NotFound($"No existe un tipo de adquisición con id {id}.");

            return NoContent();
        }

        /// <summary>
        /// Activa o desactiva (soft delete) un Tipo de Adquisición por su código.
        /// </summary>
        /// <param name="code">Código del tipo de adquisición.</param>
        /// <param name="active">Estado a asignar (true = activo, false = inactivo).</param>
        /// <response code="204">Estatus actualizado correctamente.</response>
        /// <response code="404">No se encontró el tipo de adquisición.</response>
        [HttpPatch("{code:int}/active")]
        public async Task<IActionResult> ChangeStatus(int code, [FromBody] bool active)
        {
            var success = await _mediator.Send(new UpdateStatusAcquisitionTypeCommand(code, active));

            if (!success)
                return NotFound($"No existe un tipo de adquisición con código {code}.");

            return NoContent();
        }
    }
}
