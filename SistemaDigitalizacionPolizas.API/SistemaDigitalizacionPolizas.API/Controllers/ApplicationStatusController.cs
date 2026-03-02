using Microsoft.AspNetCore.Authorization;
using MediatR;
using SistemaDigitalizacionPolizas.Application.Services.AppliccationStatus_Service.Commands.CreateApplicationStatus;
using SistemaDigitalizacionPolizas.Application.Services.AppliccationStatus_Service.Commands.UpdateApplicationStatus;
using SistemaDigitalizacionPolizas.Application.Services.AppliccationStatus_Service.Queries.GetAllApplicationStatus;
using SistemaDigitalizacionPolizas.Application.Services.AppliccationStatus_Service.Queries.GetApplicationStatusByCode;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar los estados de solicitud
    /// dentro del sistema de digitalización de pólizas.
    /// 
    /// Implementa el patrón CQRS utilizando MediatR,
    /// delegando toda la lógica de negocio a los Handlers.
    /// 
    /// Este controlador únicamente:
    /// - Recibe la petición HTTP
    /// - Envía el Command/Query al Mediator
    /// - Devuelve la respuesta correspondiente
    /// </summary>

    [Authorize] // 🔐 Se puede habilitar para proteger los endpoints con JWT
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationStatusController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor del controlador.
        /// Se inyecta IMediator para enviar comandos y consultas.
        /// </summary>
        public ApplicationStatusController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ============================================================
        // CREATE APPLICATION STATUS
        // ============================================================

        /// <summary>
        /// Crea un nuevo estado de solicitud.
        /// 
        /// Recibe un CreateApplicationStatusCommand y lo envía
        /// al handler correspondiente.
        /// 
        /// Devuelve:
        /// 201 Created → Si se crea correctamente.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> Create(
            [FromBody] CreateApplicationStatusCommand command)
        {
            var id = await _mediator.Send(command);

            // Devuelve 201 y referencia al endpoint GetByCode
            return CreatedAtAction(
                nameof(GetByCode),
                new { code = command.Code },
                id
            );
        }

        // ============================================================
        // GET BY CODE
        // ============================================================

        /// <summary>
        /// Obtiene un estado de solicitud por su código de negocio.
        /// 
        /// Devuelve:
        /// 200 OK → Si existe.
        /// 404 NotFound → Si no existe.
        /// </summary>
        [HttpGet("by-code/{code:int}")]
        public async Task<IActionResult> GetByCode(int code)
        {
            var result = await _mediator.Send(
                new GetApplicationStatusByCodeQuery(code)
            );

            if (result == null)
                return NotFound(new { message = "Estado no encontrado." });

            return Ok(result);
        }

        // ============================================================
        // GET ALL
        // ============================================================

        /// <summary>
        /// Obtiene todos los estados de solicitud.
        /// 
        /// Devuelve:
        /// 200 OK → Lista de estados.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(
                new GetAllApplicationStatusQuery()
            );

            return Ok(result);
        }

        // ============================================================
        // UPDATE (POR ID)
        // ============================================================

        /// <summary>
        /// Actualiza un estado de solicitud por su ID.
        /// 
        /// El ID se toma desde la ruta.
        /// Si el body contiene un Id diferente, se sobrescribe
        /// para garantizar consistencia REST.
        /// 
        /// Devuelve:
        /// 204 NoContent → Si se actualiza correctamente.
        /// 404 NotFound → Si no existe.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] UpdateApplicationStatusCommand command)
        {
            // Se asegura que el Id provenga de la ruta
            command = command with { IdApplicationStatus = id };

            var result = await _mediator.Send(command);

            if (!result)
                return NotFound(new { message = "Estado no encontrado." });

            return NoContent();
        }

        // ============================================================
        // UPDATE STATE (POR CODE)
        // ============================================================

        /// <summary>
        /// Activa o desactiva un estado de solicitud utilizando su código.
        /// 
        /// Solo modifica la propiedad Active.
        /// 
        /// Devuelve:
        /// 204 NoContent → Si se actualiza correctamente.
        /// 404 NotFound → Si no existe el código.
        /// </summary>
        [HttpPatch("code/{code:int}/state")]
        public async Task<IActionResult> UpdateStateByCode(
            int code,
            [FromBody] bool active)
        {
            var command = new UpdateApplicationStatusStateCommand(code, active);

            var result = await _mediator.Send(command);

            if (!result)
                return NotFound(new { message = "Estado no encontrado." });

            return NoContent();
        }
    }
}
