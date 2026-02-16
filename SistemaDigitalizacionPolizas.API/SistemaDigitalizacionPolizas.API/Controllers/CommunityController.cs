using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.Community_Service.Commands.CreateCommunity;
using SistemaDigitalizacionPolizas.Application.Services.Community_Service.Commands.UpdateCommunity;
using SistemaDigitalizacionPolizas.Application.Services.Community_Service.Commands.UpdateStatusCommunity;
using SistemaDigitalizacionPolizas.Application.Services.Community_Service.Queries.GetAllCommunity;
using SistemaDigitalizacionPolizas.Application.Services.Community_Service.Queries.GetCommunityByCode;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.UpdateFundingSource;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.UpdateStateFunding;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Queries.GetAllFundingSource;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Queries.GetByCodeFundingSource;
using SistemaDigitalizacionPolizas.Domain.Dtos.Community;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;


namespace SistemaDigitalizacionPolizas.API.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class CommunityController: ControllerBase
    {
        private readonly IMediator _mediator;

        public CommunityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Crear una comunidad
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateCommunityCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }

        // Obteiene todas las comunidades
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(
                new GetAllCommunityQuery()
            );

            return Ok(result);
        }

        // Obtener una comunidad por su código
        [HttpGet("{code:int}")]
        public async Task<ActionResult<CommunityDto>> GetByCode(int code)
        {
            var result = await _mediator.Send(new GetCommunityByCodeQuery(code));

            if (result == null)
                return NotFound($"No existe un fondo de financiamiento con código {code}");

            return Ok(result);
        }

        //Actualiza un fondo de financiamiento
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
 int id,
 [FromBody] UpdateCommunityCommand command)
        {
            // Forzamos el ID desde la URL
            var fixedCommand = command with { idCommunity = id };

            var success = await _mediator.Send(fixedCommand);

            if (!success)
                return NotFound($"No existe una oomunidad con {id}");

            return NoContent();
        }




        [HttpPatch("{code:int}/active")]
        public async Task<IActionResult> ChangeStatus(
         int code,
         [FromBody] bool active)
        {
            var success = await _mediator.Send(
                new UpdateStatusCommunityCommand(code, active)
            );

            if (!success)
                return NotFound($"No existe una comunidad con código {code}");

            return NoContent();
        }

    }
}
