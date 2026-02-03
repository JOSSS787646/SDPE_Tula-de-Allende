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

        //Crear una unidad administrativa
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateAdministrativeUnitCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
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


        //Actualizar una unidad administrativa
        [HttpPut]
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
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(int code)
        {
            var result = await _mediator.Send(
                new DeleteAdministrativeUnitCommand(code)
            );

            if (!result)
                return NotFound("Unidad administrativa no encontrada");

            return Ok(new { message = "Unidad administrativa eliminada correctamente" });
        }
    }

}
