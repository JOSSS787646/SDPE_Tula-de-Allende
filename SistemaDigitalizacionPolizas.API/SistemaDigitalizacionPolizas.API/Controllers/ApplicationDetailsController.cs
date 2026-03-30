using SistemaDigitalizacionPolizas.Application.Services.ApplicationDetails_Services.Commands.DeleteApplicationDetail;
using SistemaDigitalizacionPolizas.Application.Services.ApplicationDetails_Services.Commands.UpsertApplicationDetails;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationDetailsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationDetailsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ==========================================
        // 🔥 UPSERT MASIVO
        // ==========================================
        [HttpPost("upsert")]
        public async Task<IActionResult> UpsertDetails(
            [FromBody] UpsertApplicationDetailsCommand request)
        {
            await _mediator.Send(new UpsertApplicationDetailsCommand(
                request.RequestId,
                request.Details
            ));

            return Ok(new
            {
                message = "Detalles actualizados correctamente"
            });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteApplicationDetailCommand(id));
            return NoContent();
        }
    }
}
