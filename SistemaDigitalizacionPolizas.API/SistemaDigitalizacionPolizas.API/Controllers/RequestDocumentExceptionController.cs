using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands;
using SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands.SaveRequestDocumentExceptions;
using SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands.ToggleRequestDocumentException;
using SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands.UpsertRequestDocumentException;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Gestiona excepciones de documentos en solicitudes.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RequestDocumentExceptionController : ControllerBase
    {

        private IMediator _mediator;

        public RequestDocumentExceptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crear o actualizar una excepción de documento.
        /// </summary>
        [HttpPost("upsert")]
        public async Task<IActionResult> Upsert(
    [FromBody] UpsertRequestDocumentExceptionCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(new
            {
                message = "Excepción procesada correctamente.",
                success = result
            });
        }

        /// <summary>
        /// Activar o desactivar excepciones de documentos de forma masiva.
        /// </summary>
        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleExceptions(
            [FromBody] ToggleRequestDocumentExceptionMassCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
                return BadRequest("No se pudieron guardar las excepciones.");

            return Ok(new
            {
                message = "Excepciones guardadas correctamente"
            });
        }

    }
}