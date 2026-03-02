using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands;
using SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands.ToggleRequestDocumentException;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
   // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RequestDocumentExceptionController: ControllerBase
    {

        private IMediator _mediator;

        public RequestDocumentExceptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
    [FromBody] CreateRequestDocumentExceptionCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { message = "Excepción creada correctamente.", id });
        }
        [HttpPut("toggle")]
        public async Task<IActionResult> Toggle(
       [FromBody] ToggleRequestDocumentExceptionCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(new
            {
                message = "Estado actualizado correctamente.",
                success = result
            });
        }


    }
}
