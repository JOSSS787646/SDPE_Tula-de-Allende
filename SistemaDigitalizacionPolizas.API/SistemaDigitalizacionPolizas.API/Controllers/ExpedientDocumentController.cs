using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Queries.GetDocumentsChecklistByRequest;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.CreateExpedientDocument;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.CreateMassiveExpedientDocument;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.UpdateExpedientDocument;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Queries.GetExpedientDocumentsByClassification;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Queries.GetRequiredDocumentsByRequest;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Queries.SearchExpedientDocumentByName;
using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExpedientDocumentController: ControllerBase
    {
        private readonly IMediator _mediator;   

        public ExpedientDocumentController(IMediator mediator)
        {
            _mediator = mediator;
        }

    //    [HttpPost("upload")]
    //    [Consumes("multipart/form-data")]
    //    public async Task<IActionResult> UploadDocument(
    //[FromForm] CreateExpedientDocumentCommand command)
    //    {
    //        if (command.File == null || command.File.Length == 0)
    //            return BadRequest("File is required.");

    //        var documentId = await _mediator.Send(command);

    //        return Ok(new
    //        {
    //            message = "Documento dubido crrectamente",
    //            id = documentId
    //        });
    //    }


        [HttpPost("upload-massive")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(524288000)]
        public async Task<IActionResult> UploadMassive(
      [FromForm] int requestId,
      [FromForm] List<IFormFile> files,
      [FromForm] List<int?> documentTypeId,
      [FromForm] List<string?> observations)
        {
            if (files == null || !files.Any())
                return BadRequest("Debe enviar archivos.");

            var documents = new List<MassiveExpedientDocumentItem>();

            for (int i = 0; i < files.Count; i++)
            {
                documents.Add(new MassiveExpedientDocumentItem
                {
                    File = files[i],
                    DocumentTypeId = documentTypeId?.ElementAtOrDefault(i),
                    Observations = observations?.ElementAtOrDefault(i)
                });
            }

            var command = new CreateMassiveExpedientDocumentCommand(requestId, documents);

            var result = await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                total = result.Count,
                ids = result
            });
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search(
    [FromQuery] int requestId,
    [FromQuery] string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return BadRequest("Debe proporcionar un nombre.");

            var result = await _mediator.Send(
                new SearchExpedientDocumentByNameQuery(requestId, fileName));

            return Ok(result);
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

        [HttpGet("classification/{classificationId}")]
        public async Task<IActionResult> GetDocumentsByClassification(int classificationId)
        {
            var result = await _mediator.Send(
                new GetDocumentsByAcquisitionClassificationQuery(classificationId)
            );

            return Ok(result);
        }

        [HttpPut("update/{id}")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(524288000)] // 500MB
        public async Task<IActionResult> Update(
           int id,
           [FromForm] UpdateExpedientDocumentCommand command)
        {
            if (id <= 0)
                return BadRequest("Id inválido.");

            command.Id = id;

            var result = await _mediator.Send(command);

            return Ok(new
            {
                success = result,
                message = "Documento actualizado correctamente."
            });
        }

    }
}
