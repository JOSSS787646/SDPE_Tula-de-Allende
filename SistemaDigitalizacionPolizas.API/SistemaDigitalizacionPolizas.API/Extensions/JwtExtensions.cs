using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
/// <summary>
/// Configura la autenticación basada en JWT (JSON Web Token) para la aplicación.
/// 
/// Esta clase de extensión agrega y centraliza la configuración de seguridad
/// utilizada para validar tokens enviados en cada petición HTTP.
/// 
/// ¿Qué hace?
/// - Registra el esquema de autenticación JWT en el contenedor de servicios.
/// - Define las reglas de validación del token:
///     • Valida el emisor (Issuer)
///     • Valida la audiencia (Audience)
///     • Verifica la firma con una clave secreta
///     • Verifica la expiración del token
/// - Configura el tiempo de tolerancia (ClockSkew = 0) para evitar desfases.
/// 
/// ¿De dónde obtiene la configuración?
/// - Lee los valores desde appsettings.json en la sección "Jwt":
///     • Key → clave secreta para firmar el token
///     • Issuer → emisor válido
///     • Audience → audiencia válida
/// 
/// Caso especial (SignalR):
/// - Permite recibir el token desde query string (access_token),
///   necesario para conexiones en tiempo real (ej: /notifications).
/// 
/// ¿Cómo se usa?
/// - Se invoca en Program.cs o Startup.cs:
///     services.AddJwtAuthentication(configuration);
/// 
/// Esto habilita que los controladores protegidos con [Authorize]
/// validen automáticamente los tokens JWT en cada request.
/// </summary>
/// 


namespace SistemaDigitalizacionPolizas.API.Extensions
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
                        ),
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments("/notifications"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}