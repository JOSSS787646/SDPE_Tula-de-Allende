using SistemaDigitalizacionPolizas.Application.Services.SystemConfiguration_Service.Commands.UpsertSystemConfiguration;
using SistemaDigitalizacionPolizas.Application.Services.SystemConfiguration_Service.Queries.GetSystemConfigurationStatus;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Gestiona la configuración general del sistema.
    /// </summary>
    [ApiController]
    [Route("api/system-configuration")]
    public class SystemConfigurationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SystemConfigurationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crear o actualizar la configuración del sistema.
        /// </summary>
        [HttpPost("upsert")]
        public async Task<IActionResult> Upsert([FromBody] UpsertSystemConfigurationCommand command)
        {
            var id = await _mediator.Send(command);

            return Ok(new
            {
                message = "Configuración guardada correctamente",
                idConfiguration = id
            });
        }

        /// <summary>
        /// Obtener el estado de la configuración del sistema.
        /// </summary>
        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            var result = await _mediator.Send(new GetSystemConfigurationStatusCommand());
            return Ok(result);
        }
    }
}