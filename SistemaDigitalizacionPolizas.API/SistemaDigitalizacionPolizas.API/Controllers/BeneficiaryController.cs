using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Commands.CreateBeneficiary;
using SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Commands.UpdateBeneficiary;
using SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Commands.UpdateStatusBeneficiary;
using SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Queries.GetAllBeneficiary;
using SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Queries.GetBeneficiaryByCurp;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Gestiona los beneficiarios (crear, consultar y actualizar).
    /// </summary>
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

        /// <summary>
        /// Crear beneficiario.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBeneficiaryCommand command)
        {
            await _mediator.Send(command);

            return Ok("Beneficiario creado correctamente.");
        }

        /// <summary>
        /// Obtener todos los beneficiarios.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllBeneficiariesQuery());

            return Ok(result);
        }

        /// <summary>
        /// Obtener beneficiario por CURP.
        /// </summary>
        [HttpGet("by-curp/{curp}")]
        public async Task<IActionResult> GetByCurp(string curp)
        {
            var result = await _mediator.Send(new GetBeneficiaryByCurpQuery(curp));

            if (result is null)
                return NotFound("Beneficiario no encontrado.");

            return Ok(result);
        }

        /// <summary>
        /// Actualizar beneficiario.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBeneficiaryCommand command)
        {
            if (id != command.IdBeneficiary)
                return BadRequest("El id no coincide.");

            var success = await _mediator.Send(command);

            if (!success)
                return NotFound("No se pudo actualizar el beneficiario.");

            return NoContent();
        }

        /// <summary>
        /// Cambiar estatus (activo/inactivo) por CURP.
        /// </summary>
        [HttpPatch("by-curp/{curp}/status")]
        public async Task<IActionResult> UpdateStatusByCurp(string curp, [FromBody] bool active)
        {
            var success = await _mediator.Send(
                new UpdateBeneficiaryStatusCommand(curp, active)
            );

            if (!success)
                return NotFound("No se pudo actualizar el estatus.");

            return NoContent();
        }
    }
}