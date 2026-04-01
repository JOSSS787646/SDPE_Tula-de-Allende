using SistemaDigitalizacionPolizas.Application.Services.Auth_Service.Feature.CRUD.Command.Login;
using SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.ChangePassword;
using SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.PasswordReset;
using SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.ValidateResetCode;
using SistemaDigitalizacionPolizas.Domain.Dtos.Auth;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Gestiona la autenticación y recuperación de contraseña.
    /// </summary>
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
        /// Iniciar sesión y obtener token JWT.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var command = new LoginCommand(request.Email, request.Password);

            var response = await _mediator.Send(command);

            return Ok(response);
        }

        /// <summary>
        /// Solicitar recuperación de contraseña.
        /// </summary>
        [HttpPost("recover-password")]
        public async Task<IActionResult> RecoverPassword([FromBody] RequestPasswordResetCommand command)
        {
            await _mediator.Send(command);

            return Ok("Si el correo existe, se enviará un código de recuperación.");
        }

        /// <summary>
        /// Validar código de recuperación.
        /// </summary>
        [HttpPost("validate-code")]
        public async Task<IActionResult> ValidateCode([FromBody] ValidateResetCodeCommand command)
        {
            var isValid = await _mediator.Send(command);

            if (!isValid)
                return BadRequest("Código inválido o expirado.");

            return Ok("Código válido.");
        }

        /// <summary>
        /// Cambiar contraseña.
        /// </summary>
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var success = await _mediator.Send(command);

            if (!success)
                return BadRequest("No se pudo cambiar la contraseña.");

            return Ok("Contraseña actualizada correctamente.");
        }
    }
}