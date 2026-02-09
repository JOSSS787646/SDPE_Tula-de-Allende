using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.CreateCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateStateCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Queries.GetAllCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Queries.GetCogByCode;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.AddFundingSource;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.UpdateFundingSource;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.UpdateStateFunding;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Queries.GetAllFundingSource;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Queries.GetByCodeFundingSource;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.UpdateStatusProyect;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FundingSourceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FundingSourceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateFundingSourceCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(
                new GetAllFundingSourceQuery()
            );

            return Ok(result);
        }

        [HttpGet("{code:int}")]
        public async Task<ActionResult<FundingSourceDto>> GetByCode(int code)
        {
            var result = await _mediator.Send(new GetByCodeCommunityQuery(code));

            if (result == null)
                return NotFound($"No existe un fondo de financiamiento con código {code}");

            return Ok(result);
        }

        //Actualiza un fondo de financiamiento
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
 int id,
 [FromBody] UpdateFundingSourceCommand command)
        {
            // Forzamos el ID desde la URL
            var fixedCommand = command with { idFundingSource = id };

            var success = await _mediator.Send(fixedCommand);

            if (!success)
                return NotFound($"No existe un fondo de financiamiento con id {id}");

            return NoContent();
        }


        [HttpPatch("{code:int}/active")]
        public async Task<IActionResult> ChangeStatus(
         int code,
         [FromBody] bool active)
        {
            var success = await _mediator.Send(
                new UpdateStateFundingCommand(code, active)
            );

            if (!success)
                return NotFound($"No existe un Proyecto con código {code}");

            return NoContent();
        }



    }

}
