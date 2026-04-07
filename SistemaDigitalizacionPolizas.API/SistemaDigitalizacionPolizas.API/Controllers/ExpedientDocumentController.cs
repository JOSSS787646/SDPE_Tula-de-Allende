using Microsoft.AspNetCore.Mvc;
using MediatR;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Queries.GetDocumentsChecklistByRequest;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.AppendExpedientDocuments;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.CreateMassiveExpedientDocument;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.DeleteExpedientDocument;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.UpdateExpedientDocument;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Queries.DownloadDocument;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Queries.GetExpedientDocumentsByClassification;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Queries.SearchExpedientDocumentByName;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.ReviewDocument;
using SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument;
using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Dtos.Auth;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Gestiona documentos del expediente (consulta, carga, actualización y eliminación).
    /// </summary>
    [ApiController]
    [Route("api/expedient-documents")]
    public class ExpedientDocumentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExpedientDocumentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtener checklist de documentos por solicitud.
        /// </summary>
        [HttpGet("requests/{requestId}/checklist")]
        public async Task<ActionResult<List<RequestDocumentChecklistDto>>> GetChecklist(int requestId)
        {
            var result = await _mediator.Send(new GetDocumentsChecklistByRequestQuery(requestId));

            return Ok(result);
        }

        /// <summary>
        /// Obtener documentos por clasificación (paginado).
        /// </summary>
        [HttpGet("classification")]
        public async Task<IActionResult> GetByClassification(int classificationId, int page = 1, int pageSize = 10)
        {
            var result = await _mediator.Send(
                new GetExpedientDocumentsByClassificationQuery(classificationId, page, pageSize)
            );

            return Ok(result);
        }

        /// <summary>
        /// Buscar documentos por nombre.
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> Search(int requestId, string fileName)
        {
            var result = await _mediator.Send(
                new SearchExpedientDocumentByNameQuery(requestId, fileName)
            );

            return Ok(result);
        }

        /// <summary>
        /// Descargar documento.
        /// </summary>
        [HttpGet("{id:int}/download")]
        public async Task<IActionResult> Download(int id)
        {
            var file = await _mediator.Send(new DownloadDocumentQuery(id));

            return File(file.FileStream, file.ContentType, file.FileName);
        }

        /// <summary>
        /// Subir documentos masivos.
        /// </summary>
        [HttpPost("upload-massive")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadMassive(
            [FromForm] int requestId,
            [FromForm] List<IFormFile> files,
            [FromForm] List<int?> documentTypeId,
            [FromForm] List<string?> observations)
        {
            var documents = files.Select((file, i) => new MassiveExpedientDocumentItem
            {
                File = file,
                DocumentTypeId = documentTypeId?.ElementAtOrDefault(i),
                Observations = observations?.ElementAtOrDefault(i)
            }).ToList();

            var result = await _mediator.Send(
                new CreateMassiveExpedientDocumentCommand(requestId, documents)
            );

            return Ok(result);
        }

        /// <summary>
        /// Agregar archivos a un tipo de documento.
        /// </summary>
        [HttpPost("requests/{requestId}/documents/{documentTypeId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Append(int requestId, int documentTypeId,
            [FromForm] List<IFormFile> files,
            [FromForm] string? observations)
        {
            var result = await _mediator.Send(
                new AppendExpedientDocumentsCommand(requestId, documentTypeId, files, observations)
            );

            return Ok(result);
        }

        /// <summary>
        /// Actualizar documento.
        /// </summary>
        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateExpedientDocumentCommand command)
        {
            command.Id = id;

            var success = await _mediator.Send(command);

            if (!success)
                return NotFound("Documento no encontrado.");

            return NoContent();
        }

        /// <summary>
        /// Eliminar documento.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, [FromBody] DeleteExpedientDocumentRequestDto request)
        {
            var result = await _mediator.Send(
                new DeleteExpedientDocumentCommand(id, request.Password)
            );

            return Ok(result);
        }

        /// <summary>
        /// Revisar documento (estatus y observaciones).
        /// </summary>
        [HttpPost("{id:int}/review")]
        public async Task<IActionResult> Review(int id, [FromBody] ReviewDocumentRequestDto request)
        {
            var result = await _mediator.Send(
                new ReviewDocumentCommand(id, request.DocumentStatusId, request.Observations)
            );

            return Ok(result);
        }
    }
}