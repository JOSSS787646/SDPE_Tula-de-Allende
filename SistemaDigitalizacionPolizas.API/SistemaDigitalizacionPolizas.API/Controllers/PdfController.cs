using SistemaDigitalizacionPolizas.Application.Services.Pdf_Service.Commands.GenerateAcquisitionRequestFormPdf;
using SistemaDigitalizacionPolizas.Application.Services.Pdf_Service.Commands.GenerateChecklistPdf;
using SistemaDigitalizacionPolizas.Application.Services.Pdf_Service.Query.GetAcquisitionRequestPdf;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PdfController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 🔥 GET: api/pdf/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPdf(int id)
        {
            var pdfBytes = await _mediator.Send(
                new GetAcquisitionRequestPdfCommand(id)
            );

            return File(
                pdfBytes,
                "application/pdf",
                $"AcquisitionRequest_{id}.pdf"
            );
        }


        [HttpGet("form/{id}")]
        public async Task<IActionResult> GetFormPdf(int id)
        {
            var pdf = await _mediator.Send(
                new GenerateAcquisitionRequestFormPdfCommand(id)
            );

            return File(pdf, "application/pdf", $"Formulario_{id}.pdf");
        }


        /// <summary>
        /// 📄 Genera y descarga el PDF del checklist de documentos
        /// </summary>
        [HttpGet("checklist/{requestId}")]
        public async Task<IActionResult> GetChecklistPdf(int requestId)
        {
            var pdfBytes = await _mediator
                .Send(new GenerateChecklistPdfCommand(requestId));

            if (pdfBytes == null || pdfBytes.Length == 0)
                return NotFound("No se pudo generar el PDF");

            return File(
                pdfBytes,
                "application/pdf",
                $"Checklist_{requestId}.pdf"
            );
        }
    }

}
