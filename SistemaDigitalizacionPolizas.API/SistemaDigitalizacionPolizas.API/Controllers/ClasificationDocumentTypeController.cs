using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.ClasificationDocumentType_Service.Comannds.CreateClasificationDocumentType;
using SistemaDigitalizacionPolizas.Application.Services.ClasificationDocumentType_Service.Queries.GetDocumentByClassification;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Gestiona la asignación y consulta de tipos de documento por clasificación.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClasificationDocumentTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClasificationDocumentTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Asignar documentos a una clasificación (reemplazo masivo).
        /// </summary>
        [HttpPost("assign")]
        public async Task<IActionResult> AssignDocuments([FromBody] AssignDocumentsToClassificationCommand command)
        {
            var success = await _mediator.Send(command);

            if (!success)
                return BadRequest("No se pudieron asignar los documentos.");

            return Ok("Documentos asignados correctamente.");
        }

        /// <summary>
        /// Obtener documentos por clasificación.
        /// </summary>
        [HttpGet("by-classification/{acquisitionClassificationId:int}")]
        public async Task<IActionResult> GetByClassification(int acquisitionClassificationId)
        {
            var result = await _mediator.Send(
                new GetDocumentsByClassificationQuery(acquisitionClassificationId)
            );

            if (result is null || !result.Any())
                return NotFound("No hay documentos para esta clasificación.");

            return Ok(result);
        }
    }
}