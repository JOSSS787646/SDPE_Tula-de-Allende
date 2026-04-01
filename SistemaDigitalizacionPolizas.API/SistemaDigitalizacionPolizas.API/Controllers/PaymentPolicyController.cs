using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Commands.CreatePaymentPolicy;
using SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Commands.DeletePaymentPolicy;
using SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Commands.UpdatePaymentPolicy;
using SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Queries.DownloadPaymentPolicy;
using SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Queries.GetAllPaymentPolicies;
using SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Queries.GetAvailablePaymentPolicies;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> GetAllPolicies(
     [FromQuery] int page = 1,
     [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(
                new GetPaymentPoliciesQuery(page, pageSize)
            );

            return Ok(result);
        }

        [HttpGet("available-policies")]
        public async Task<IActionResult> GetAvailablePolicies()
        {
            var result = await _mediator.Send(new GetAvailablePaymentPoliciesQuery());

            return Ok(result);
        }

        [HttpPut("payment-policies")]
        public async Task<IActionResult> UpdatePaymentPolicy(
    [FromForm] UpdatePaymentPolicyCommand command)
        {
            var result = await _mediator.Send(command);


            return Ok(result);
        }


      

        [HttpGet("payment-policies/{id}/download")]
        public async Task<IActionResult> DownloadPaymentPolicy(int id)
        {
            var result = await _mediator.Send(new DownloadPaymentPolicyQuery(id));

            return File(
                result.FileStream,
                result.ContentType,
                result.FileName
            );
        }
    }
}

