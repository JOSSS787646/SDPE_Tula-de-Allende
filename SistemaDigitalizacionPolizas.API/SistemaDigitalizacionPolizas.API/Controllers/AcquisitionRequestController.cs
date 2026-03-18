using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.CreateRequest;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.DeleteRequest;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.UpdateAcqusitionRequest;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.UpdateCFDI;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.UpdateMaxDate;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.UpdatePaymentPolicy;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Queries.GetAcquisitionRequestDetail;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Queries.GetAllAcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
   // [Authorize]
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
        public async Task<IActionResult> Create([FromBody] CreateAcquisitionRequestDto dto)
        {
            var idRequest = await _mediator.Send(new CreateRequestCommand(dto));

            return Ok(new
            {
                success = true,
                idRequest = idRequest,
                message = "Solicitud creada correctamente."
            });
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


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateAcquisitionRequestDto dto)
        {
            // 🔥 Si el cuerpo viene con 0 o diferente, usamos el de la URL
            dto.IdRequest = id;

            var result = await _mediator.Send(
                new UpdateAcquisitionRequestCommand(dto));

            if (!result)
                return NotFound();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
     int id,
     [FromBody] DeleteRequestRequestDto request)
        {
            await _mediator.Send(
                new DeleteRequestCommand(id, request.Password)
            );

            return NoContent();
        }


        [HttpPatch("{id}/payment-policy")]
        public async Task<IActionResult> UpdatePaymentPolicy(
    int id,
    [FromBody] int idPaymentPolicy)
        {
            var result = await _mediator.Send(
                new UpdatePaymentPolicyCommand(id, idPaymentPolicy)
            );

            return Ok(result);
        }

        [HttpPatch("{id}/CFDI")]
        public async Task<IActionResult> UpdateCFDICommand(
   int id,
   [FromBody] string cdfi)
        {
            var result = await _mediator.Send(
                new UpdateCFDICommand(id, cdfi)
            );

            return Ok(result);
        }


        [HttpPatch("update-max-date")]
        public async Task<IActionResult> UpdateMaxDate(UpdateMaxDateDto dto)
        {
            var command = new UpdateMaxDateCommand
            {
                RequestId = dto.RequestId,
                NewMaxDate = dto.NewMaxDate
            };

            await _mediator.Send(command);

            return Ok(new { message = "Fecha actualizada correctamente" });
        }
    }
}
