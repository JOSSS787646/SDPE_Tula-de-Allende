using SistemaDigitalizacionPolizas.Application.Services.SystemConfiguration_Service.Commands.UpsertSystemConfiguration;
using SistemaDigitalizacionPolizas.Application.Services.SystemConfiguration_Service.Queries.GetSystemConfigurationStatus;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    [ApiController]
    [Route("api/system-configuration")]
    public class SystemConfigurationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SystemConfigurationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upsert")]
        public async Task<IActionResult> Upsert([FromBody] UpsertSystemConfigurationCommand command)
        {
            var id = await _mediator.Send(command);

            return Ok(new
            {
                message = "Configuration saved successfully",
                idConfiguration = id
            });
        }


        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            var result = await _mediator.Send(new GetSystemConfigurationStatusCommand());
            return Ok(result);
        }
    }
}
