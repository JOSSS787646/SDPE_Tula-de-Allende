using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.CreateExpedientDocument;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpedientDocumentController: ControllerBase
    {
        private readonly IMediator _mediator;   

        public ExpedientDocumentController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDocument(
        [FromForm] CreateExpedientDocumentCommand command)
        {
            if (command.File == null || command.File.Length == 0)
                return BadRequest("File is required.");

            var documentId = await _mediator.Send(command);

            return Ok(new
            {
                message = "Document uploaded successfully",
                id = documentId
            });
        }

    }
}
