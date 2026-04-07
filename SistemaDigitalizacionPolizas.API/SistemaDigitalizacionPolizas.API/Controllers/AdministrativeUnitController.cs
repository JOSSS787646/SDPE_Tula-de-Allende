using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.CreatedAdministrativeUnit;
using SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.DeleteAdministrativeUnit;
using SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.UpdateAAdministrativeUnits;
using SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Queries.GetAllAdministrativeUnit;
using SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Queries.GeyByIdAdministrativeUnit;
using SistemaDigitalizacionPolizas.Domain.Dtos.AdministrativeUnit;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdministrativeUnitController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdministrativeUnitController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crear una unidad administrativa.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateAdministrativeUnitCommand command)
        {
            var id = await _mediator.Send(command);

            if (id == 0)
                return Conflict("Ya existe una unidad administrativa con ese código.");

            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }

        /// <summary>
        /// Obtener unidad administrativa por código.
        /// </summary>
        [HttpGet("{code:int}")]
        public async Task<IActionResult> GetByCode(int code)
        {
            var result = await _mediator.Send(new GetAdministrativeUnitByCodeQuery(code));

            if (result is null)
                return NotFound("Unidad administrativa no encontrada.");

            return Ok(result);
        }

        /// <summary>
        /// Obtener todas las unidades administrativas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllAdministrativeUnitQuery());

            return Ok(result);
        }

        /// <summary>
        /// Actualizar unidad administrativa.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AdministrativeUnitDto dto)
        {
            var success = await _mediator.Send(new UpdateAdministrativeUnitCommand(id, dto));

            if (!success)
                return NotFound($"No existe una unidad administrativa con id {id}.");

            return NoContent();
        }

        /// <summary>
        /// Cambiar estatus (activo/inactivo) de la unidad administrativa.
        /// </summary>
        [HttpPatch("{code:int}/status")]
        public async Task<IActionResult> ChangeStatus(int code, [FromBody] bool active)
        {
            var success = await _mediator.Send(new DeleteAdministrativeUnitCommand(code, active));

            if (!success)
                return NotFound($"No existe una unidad administrativa con código {code}.");

            return NoContent();
        }
    }
}