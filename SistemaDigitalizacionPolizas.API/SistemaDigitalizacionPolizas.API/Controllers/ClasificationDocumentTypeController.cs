using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.ClasificationDocumentType_Service.Comannds.CreateClasificationDocumentType;
using SistemaDigitalizacionPolizas.Application.Services.ClasificationDocumentType_Service.Queries.GetDocumentByClassification;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClasificationDocumentTypeController: ControllerBase
    {


        private readonly IMediator _mediator;

        public ClasificationDocumentTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Asigna múltiples tipos de documento a una clasificación
        /// (carga masiva - reemplaza los actuales).
        /// </summary>
        [HttpPost("assign")]
        public async Task<IActionResult> AssignDocuments(
            [FromBody] AssignDocumentsToClassificationCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            if (!result)
                return BadRequest("No se pudieron asignar los documentos.");

            return Ok(new
            {
                message = "Documentos asignados correctamente."
            });
        }

        /// <summary>
        /// Obtiene todos los tipos de documento asignados a una clasificación de adquisición
        /// </summary>
        [HttpGet("by-classification/{acquisitionClassificationId}")]
        public async Task<IActionResult> GetByClassification(int acquisitionClassificationId)
        {
            if (acquisitionClassificationId <= 0)
                return BadRequest("El id de la clasificación es inválido.");

            var result = await _mediator.Send(
                new GetDocumentsByClassificationQuery(acquisitionClassificationId));

            if (result == null || !result.Any())
                return NotFound();
                    
            return Ok(result);
        }



    }
}
