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
    }

}
