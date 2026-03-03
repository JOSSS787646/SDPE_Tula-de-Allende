using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.CreateRequest;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Queries.GetAcquisitionRequestDetail;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Queries.GetAllAcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;

namespace SistemaDigitalizacionPolizas.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AcquisitionRequestController : ControllerBase
    {
        public IMediator _mediator;

        public AcquisitionRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ==========================================
        // CREATE REQUEST
        // ==========================================
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateAcquisitionRequestDto dto)
        {
            await _mediator.Send(new CreateRequestCommand(dto));

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _mediator.Send(
                new GetAllAcquisitionRequestPolizaCommand(pageNumber, pageSize));

            return Ok(result);
        }

        // 🔥 GET: api/AcquisitionRequest/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AcquisitionRequestDetailDto>> GetDetail(int id)
        {
            var result = await _mediator.Send(new GetAcquisitionRequestDetailQuery(id));

            if (result == null)
                return NotFound("No se encontró la solicitud.");

            return Ok(result);
        }
    }
}
