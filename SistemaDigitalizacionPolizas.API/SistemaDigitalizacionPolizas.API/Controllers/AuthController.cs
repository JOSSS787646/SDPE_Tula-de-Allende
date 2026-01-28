

using SistemaDigitalizacionPolizas.Application.Services.Auth_Service.Feature.CRUD.Command.Login;
using SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.ChangePassword;
using SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.PasswordReset;
using SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.ValidateResetCode;
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


        //Recupera la contraseña del usuario enviando un correo con un código de recuperación
        [HttpPost("recover-password")]
        public async Task<IActionResult> RecoverPassword([FromBody] RequestPasswordResetCommand command)
        {
            await _mediator.Send(command);

            return Ok("Si el correo existe, se enviará un código de recuperación.");
        }

        // Valida el código de recuperación de contraseña
        [HttpPost("validate-code")]
        public async Task<IActionResult> ValidateCode(
        [FromBody] ValidateResetCodeCommand command)
        {
            var isValid = await _mediator.Send(command);

            if (!isValid)
                return BadRequest("Código inválido o expirado");

            return Ok("Código válido");
        }

        //Cambia la contraseña del usuario una vez que ha validado el código de recuperación
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(
         [FromBody] ChangePasswordCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
                return BadRequest("No se pudo cambiar la contraseña.");

            return Ok("Contraseña actualizada correctamente.");
        }


    }
}
