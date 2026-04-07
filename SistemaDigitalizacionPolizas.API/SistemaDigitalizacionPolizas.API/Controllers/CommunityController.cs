using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.Community_Service.Commands.CreateCommunity;
using SistemaDigitalizacionPolizas.Application.Services.Community_Service.Commands.UpdateCommunity;
using SistemaDigitalizacionPolizas.Application.Services.Community_Service.Commands.UpdateStatusCommunity;
using SistemaDigitalizacionPolizas.Application.Services.Community_Service.Queries.GetAllCommunity;
using SistemaDigitalizacionPolizas.Application.Services.Community_Service.Queries.GetCommunityByCode;
using SistemaDigitalizacionPolizas.Domain.Dtos.Community;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Gestiona comunidades: creación, consulta, actualización y cambio de estatus.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommunityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommunityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crear una comunidad.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateCommunityCommand command)
        {
            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetByCode), new { code = command.Code }, id);
        }

        /// <summary>
        /// Obtener todas las comunidades registradas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllCommunityQuery());

            return Ok(result);
        }

        /// <summary>
        /// Obtener una comunidad por su código.
        /// </summary>
        [HttpGet("{code:int}")]
        public async Task<ActionResult<CommunityDto>> GetByCode(int code)
        {
            var result = await _mediator.Send(new GetCommunityByCodeQuery(code));

            if (result is null)
                return NotFound($"No existe una comunidad con código {code}.");

            return Ok(result);
        }

        /// <summary>
        /// Actualizar una comunidad por id.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommunityCommand command)
        {
            var fixedCommand = command with { idCommunity = id };

            var success = await _mediator.Send(fixedCommand);

            if (!success)
                return NotFound($"No existe una comunidad con id {id}.");

            return NoContent();
        }

        /// <summary>
        /// Cambiar estatus (activo/inactivo) de una comunidad.
        /// </summary>
        [HttpPatch("{code:int}/status")]
        public async Task<IActionResult> ChangeStatus(int code, [FromBody] bool active)
        {
            var success = await _mediator.Send(
                new UpdateStatusCommunityCommand(code, active)
            );

            if (!success)
                return NotFound($"No existe una comunidad con código {code}.");

            return NoContent();
        }
    }
}