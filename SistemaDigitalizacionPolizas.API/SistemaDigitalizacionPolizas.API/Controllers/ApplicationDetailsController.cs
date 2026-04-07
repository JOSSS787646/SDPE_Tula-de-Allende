using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.ApplicationDetails_Services.Commands.DeleteApplicationDetail;
using SistemaDigitalizacionPolizas.Application.Services.ApplicationDetails_Services.Commands.UpsertApplicationDetails;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar los detalles de aplicación,
    /// permitiendo crear, actualizar (upsert) y eliminar registros asociados a una solicitud.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationDetailsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationDetailsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crear o actualizar (upsert) detalles de aplicación de forma masiva.
        /// Si los registros existen, los actualiza; si no, los crea.
        /// </summary>
        [HttpPost("upsert")]
        public async Task<IActionResult> UpsertDetails([FromBody] UpsertApplicationDetailsCommand request)
        {
            await _mediator.Send(new UpsertApplicationDetailsCommand(
                request.RequestId,
                request.Details
            ));

            return Ok(new
            {
                message = "Detalles actualizados correctamente."
            });
        }

        /// <summary>
        /// Eliminar un detalle de aplicación por id.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteApplicationDetailCommand(id));

            return NoContent();
        }
    }
}