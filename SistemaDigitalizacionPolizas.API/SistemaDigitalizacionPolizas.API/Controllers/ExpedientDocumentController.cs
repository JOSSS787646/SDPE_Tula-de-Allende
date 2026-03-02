using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Queries.GetDocumentsChecklistByRequest;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.CreateExpedientDocument;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Queries.GetExpedientDocumentsByClassification;
using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument;

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
                message = "Documento dubido crrectamente",
                id = documentId
            });
        }

        [HttpGet("by-classification")]
        public async Task<ActionResult<List<ExpedientDocumentDto>>> GetByClassification(
           [FromQuery] int classificationId,
           [FromQuery] int page = 1,
           [FromQuery] int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page and PageSize must be greater than 0.");

            var query = new GetExpedientDocumentsByClassificationQuery(
                classificationId,
                page,
                pageSize
            );

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("{id}/documents-checklist")]
        public async Task<ActionResult<List<RequestDocumentChecklistDto>>> GetDocumentsChecklist(int id)
        {
            var result = await _mediator.Send(new GetDocumentsChecklistByRequestQuery(id));
            return Ok(result);
        }

    }
}
