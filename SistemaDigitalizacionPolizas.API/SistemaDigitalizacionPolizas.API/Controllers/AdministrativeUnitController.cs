using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.CreatedAdministrativeUnit;
using SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.DeleteAdministrativeUnit;
using SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.UpdateAAdministrativeUnits;
using SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Queries.GetAllAdministrativeUnit;
using SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Queries.GeyByIdAdministrativeUnit;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateStateCog;
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

        // Crear una unidad administrativa
        [HttpPost]
        public async Task<ActionResult<int>> Create(
            [FromBody] CreateAdministrativeUnitCommand command)
        {
            var id = await _mediator.Send(command);

            if (id == 0)
                return Conflict("Ya existe una unidad administrativa con ese código");

            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }



        //Obtener una unidad administrativa por su id
        [HttpGet("{code:int}")]
        public async Task<IActionResult> GetByCode(int code)
        {
            var result = await _mediator.Send(
                new GetAdministrativeUnitByCodeQuery(code)
            );

            if (result == null)
                return NotFound("Unidad administrativa no encontrada");

            return Ok(result);
        }

        //Obtener todas las unidades administrativas
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(
                new GetAllAdministrativeUnitQuery()
            );

            return Ok(result);
        }


        // Actualizar una unidad administrativa
        [HttpPut("{code:int}")]
        public async Task<IActionResult> Update(
            int code,
            [FromBody] AdministrativeUnitDto dto)
        {
            var result = await _mediator.Send(
                new UpdateAdministrativeUnitCommand(code, dto)
            );

            if (!result)
                return NotFound("Unidad administrativa no encontrada");

            return Ok("Unidad administrativa actualizada correctamente");
        }

        //Eliminar una unidad administrativa

        [HttpPatch("{code:int}/active")]
        public async Task<IActionResult> ChangeStatus(
      int code,
      [FromBody] bool active)
        {
            var success = await _mediator.Send(
                new DeleteAdministrativeUnitCommand(code, active)
            );

            if (!success)
                return NotFound($"No existe un Proyecto con código {code}");

            return NoContent();
        }
    }

}
