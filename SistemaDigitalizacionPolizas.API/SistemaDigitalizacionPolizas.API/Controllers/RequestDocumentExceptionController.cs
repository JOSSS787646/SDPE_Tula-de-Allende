using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands;
using SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands.SaveRequestDocumentExceptions;
using SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands.ToggleRequestDocumentException;
using SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands.UpsertRequestDocumentException;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RequestDocumentExceptionController: ControllerBase
    {

        private IMediator _mediator;

        public RequestDocumentExceptionController(IMediator mediator)
        {
            _mediator = mediator;
        }


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

        //    [HttpPost]
        //    public async Task<IActionResult> Create(
        //[FromBody] CreateRequestDocumentExceptionCommand command)
        //    {
        //        var id = await _mediator.Send(command);
        //        return Ok(new { message = "Excepción creada correctamente.", id });
        //    }
        //    [HttpPut("toggle")]
        //    public async Task<IActionResult> Toggle(
        //   [FromBody] ToggleRequestDocumentExceptionCommand command)
        //    {
        //        var result = await _mediator.Send(command);

        //        return Ok(new
        //        {
        //            message = "Estado actualizado correctamente.",
        //            success = result
        //        });
        //    }


    }
}
