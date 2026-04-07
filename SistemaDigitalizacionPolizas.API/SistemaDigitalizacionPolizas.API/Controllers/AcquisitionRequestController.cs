using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.CreateRequest;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.DeleteRequest;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.ProcessExpiredRequests;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.UpdateAcqusitionRequest;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.UpdateCFDI;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.UpdateMaxDate;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.UpdatePaymentPolicy;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Queries.GetAcquisitionRequestDetail;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Queries.GetAllAcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar solicitudes de adquisición (Solicitudes).
    /// Permite crear, consultar, actualizar y ejecutar procesos relacionados.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AcquisitionRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AcquisitionRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crea una nueva solicitud.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAcquisitionRequestDto dto)
        {
            var idRequest = await _mediator.Send(new CreateRequestCommand(dto));

            return Ok(new
            {
                success = true,
                idRequest,
                message = "Solicitud creada correctamente."
            });
        }

        /// <summary>
        /// Obtiene listado paginado de solicitudes.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _mediator.Send(
                new GetAllAcquisitionRequestPolizaCommand(pageNumber, pageSize));

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el detalle de una solicitud por Id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AcquisitionRequestDetailDto>> GetDetail(int id)
        {
            var result = await _mediator.Send(new GetAcquisitionRequestDetailQuery(id));

            if (result == null)
                return NotFound("No se encontró la solicitud.");

            return Ok(result);
        }

        /// <summary>
        /// Actualiza una solicitud existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateAcquisitionRequestDto dto)
        {
            // Asegura que el Id venga desde la URL
            dto.IdRequest = id;

            var result = await _mediator.Send(
                new UpdateAcquisitionRequestCommand(dto));

            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Elimina una solicitud (requiere contraseña).
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromBody] DeleteRequestRequestDto request)
        {
            await _mediator.Send(
                new DeleteRequestCommand(id, request.Password));

            return NoContent();
        }

        /// <summary>
        /// Actualiza la política de pago de una solicitud.
        /// </summary>
        [HttpPatch("{id}/payment-policy")]
        public async Task<IActionResult> UpdatePaymentPolicy(int id, [FromBody] int idPaymentPolicy)
        {
            var result = await _mediator.Send(
                new UpdatePaymentPolicyCommand(id, idPaymentPolicy));

            return Ok(result);
        }

        /// <summary>
        /// Actualiza el CFDI de una solicitud.
        /// </summary>
        [HttpPatch("{id}/CFDI")]
        public async Task<IActionResult> UpdateCFDICommand(int id, [FromBody] string cdfi)
        {
            var result = await _mediator.Send(
                new UpdateCFDICommand(id, cdfi));

            return Ok(result);
        }

        /// <summary>
        /// Actualiza la fecha máxima de una solicitud.
        /// </summary>
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

        /// <summary>
        /// Ejecuta el proceso de solicitudes expiradas (uso manual o pruebas).
        /// </summary>
        [HttpPost("test-expired-requests")]
        public async Task<IActionResult> TestExpiredRequests()
        {
            await _mediator.Send(new ProcessExpiredRequestsCommand());
            return Ok("Ejecutado");
        }
    }
}