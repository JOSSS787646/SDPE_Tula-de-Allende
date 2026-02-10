using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Commands.CreateBeneficiary;
using SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Commands.UpdateBeneficiary;
using SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Commands.UpdateStatusBeneficiary;
using SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Queries.GetAllBeneficiary;
using SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Queries.GetBeneficiaryByCurp;

namespace SistemaDigitalizacionPolizas.API.Controllers
{


    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BeneficiaryController : ControllerBase
    {

        private readonly IMediator _mediator;

        public BeneficiaryController(IMediator mediator)
        {
            _mediator = mediator;


        }

        // POST: api/Beneficiary
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CreateBeneficiaryCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { message = "Beneficiario creado correctamente." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllBeneficiariesQuery());
            return Ok(result);
        }

        [HttpGet("by-curp/{curp}")]
        public async Task<IActionResult> GetByCurp(string curp)
        {
            var result = await _mediator.Send(new GetBeneficiaryByCurpQuery(curp));

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        // PUT: api/Beneficiary/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBeneficiaryCommand command)
        {
            if (id != command.IdBeneficiary)
                return BadRequest("El id de la ruta no coincide con el del cuerpo.");

            var success = await _mediator.Send(command);

            if (!success)
                return NotFound("No se pudo actualizar el beneficiario.");

            return Ok(new { message = "Beneficiario actualizado correctamente." });
        }

        // PATCH: api/Beneficiary/by-curp/{curp}/status
        [HttpPatch("by-curp/{curp}/status")]
        public async Task<IActionResult> UpdateStatusByCurp(string curp, [FromBody] bool active)
        {
            var success = await _mediator.Send(
                new UpdateBeneficiaryStatusCommand(curp, active)
            );

            if (!success)
                return NotFound("No se pudo actualizar el estatus del beneficiario.");

            return Ok(new
            {
                message = $"Beneficiario {(active ? "activado" : "desactivado")} correctamente."
            });
        }





    }
}
