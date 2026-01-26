

using SistemaDigitalizacionPolizas.Application.Services.Auth_Service.Feature.CRUD.Command.Login;
using SistemaDigitalizacionPolizas.Domain.Dtos.Auth;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Inicia sesión y genera el token JWT
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var command = new LoginCommand(
                request.Email,
                request.Password
            );

            var response = await _mediator.Send(command);

            return Ok(response);
        }
    }
}
