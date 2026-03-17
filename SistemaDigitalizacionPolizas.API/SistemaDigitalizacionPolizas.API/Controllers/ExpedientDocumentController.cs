using Microsoft.AspNetCore.Authorization;
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
using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument;
using SistemaDigitalizacionPolizas.Domain.Dtos.Auth;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.ReviewDocument;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Controlador para la gestión de documentos del expediente.
    /// Permite consultar, subir, actualizar, eliminar y descargar documentos.
    /// </summary>
   // [Authorize]
    [ApiController]
    [Route("api/expedient-documents")]
    public class ExpedientDocumentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExpedientDocumentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ============================================================
        // CONSULTAS (GET)
        // ============================================================

        /// <summary>
        /// Obtiene los documentos requeridos (checklist) para una solicitud.
        /// </summary>
        [HttpGet("requests/{requestId}/checklist")]
        public async Task<ActionResult<List<RequestDocumentChecklistDto>>> GetDocumentsChecklist(int requestId)
        {
            var result = await _mediator.Send(new GetDocumentsChecklistByRequestQuery(requestId));
            return Ok(result);
        }

        /// <summary>
        /// Obtiene documentos filtrados por clasificación con paginación.
        /// </summary>
        [HttpGet("classification")]
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

        /// <summary>
        /// Busca documentos dentro de una solicitud por nombre de archivo.
        /// </summary>
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

        /// <summary>
        /// Descarga un documento del expediente.
        /// </summary>
        [HttpGet("{expedientDocumentId}/download")]
        public async Task<IActionResult> DownloadDocument(int expedientDocumentId)
        {
            var file = await _mediator.Send(new DownloadDocumentQuery(expedientDocumentId));

            return File(file.FileStream, file.ContentType, file.FileName);
        }

        // ============================================================
        // CREACIÓN / SUBIDA (POST)
        // ============================================================

        /// <summary>
        /// Sube múltiples documentos a una solicitud.
        /// </summary>
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

        /// <summary>
        /// Agrega archivos adicionales a un tipo de documento existente.
        /// </summary>
        [HttpPost("requests/{requestId}/documents/{documentTypeId}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<List<int>>> AppendDocuments(
            int requestId,
            int documentTypeId,
            [FromForm] List<IFormFile> files,
            [FromForm] string? observations)
        {
            var command = new AppendExpedientDocumentsCommand(
                requestId,
                documentTypeId,
                files,
                observations
            );

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        // ============================================================
        // ACTUALIZACIÓN (PUT)
        // ============================================================

        /// <summary>
        /// Actualiza un documento existente del expediente.
        /// </summary>
        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(52428800)]
        public async Task<IActionResult> UpdateDocument(
        int id,
        [FromForm] UpdateExpedientDocumentCommand command)
        {
            if (id <= 0)
                return BadRequest("Id inválido.");

            command.Id = id;

            var result = await _mediator.Send(command);

            if (!result)
                return NotFound("Documento no encontrado.");

            return Ok("Documento actualizado correctamente.");
        }

        // ============================================================
        // ELIMINACIÓN (DELETE)
        // ============================================================

        /// <summary>
        /// Elimina un documento del expediente.
        /// Se requiere contraseña para confirmar la eliminación.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpedientDocument(
            int id,
            [FromBody] DeleteExpedientDocumentRequestDto request)
        {
            var result = await _mediator.Send(
                new DeleteExpedientDocumentCommand(id, request.Password)
            );

            return Ok(result);
        }




        [HttpPost("expedient-documents/{id}/review")]
        public async Task<IActionResult> ReviewDocument(
    int id,
    [FromBody] ReviewDocumentRequestDto request)
        {
            var result = await _mediator.Send(
                new ReviewDocumentCommand(
                    id,
                    request.DocumentStatusId,
                    request.Observations
                )
            );

            return Ok(result);
        }
    }
}