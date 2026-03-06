using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Commands.CreatePaymentPolicy;
using SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Queries.GetAllPaymentPolicies;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentPolicyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentPolicyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crear una póliza de pago y subir el archivo PDF
        /// </summary>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreatePaymentPolicy(
            [FromForm] CreatePaymentPolicyCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(new
            {
                message = "Payment policy created successfully",
                id = result
            });
        }

        [HttpGet("policies")]
        public async Task<IActionResult> GetAllPolicies()
        {
            var result = await _mediator.Send(new GetAllPaymentPoliciesQuery());

            return Ok(result);
        }
    }
}