using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;


/// <summary>
/// Implementación del servicio de generación de tokens JWT.
///
/// Se encarga de crear tokens de autenticación firmados que contienen
/// la información del usuario (claims), permitiendo su identificación
/// y autorización en el sistema.
///
/// Responsabilidad:
/// - Construir los claims del usuario (Id, Email, Rol).
/// - Generar y firmar el token usando una clave secreta.
/// - Definir la expiración del token.
/// - Retornar el token junto con su fecha de expiración.
///
/// Funcionamiento:
/// - Toma los datos del usuario.
/// - Crea una lista de claims con su información relevante.
/// - Genera una clave de seguridad a partir de la configuración.
/// - Firma el token con algoritmo HmacSha256.
/// - Construye el JWT con issuer, audience y expiración.
/// - Devuelve el token serializado.
///
/// Dependencias:
/// - JwtSettings: configuración del token (clave, issuer, audience, expiración).
///
/// Uso:
/// Se utiliza durante el proceso de autenticación para generar
/// el token que será enviado al cliente y usado en futuras solicitudes.
///
/// Nota:
/// El token incluye múltiples claims de rol (nombre y Id),
/// lo que permite flexibilidad en validaciones posteriores.
/// </summary>
/// 

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.Auth
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;

        public JwtService(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }
        


        
    /// <summary>
    /// Genera un token JWT para el usuario especificado.
    /// </summary>
    /// 

        public JwtResult GenerateToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.RolName),
            new Claim(ClaimTypes.Role, user.IdRole.ToString())


        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_settings.Key)
            );

            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256
            );

            var expires = DateTime.UtcNow.AddHours(_settings.ExpiresInHours);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expires
            };
        }
    }

}
