using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;


/// <summary>
/// Servicio que obtiene la información del usuario autenticado actual
/// desde el HttpContext.
///
/// Permite acceder al Id, correo y rol del usuario a partir de los claims
/// del token JWT, sin depender directamente del HttpContext en otras capas.
///
/// Se utiliza principalmente para auditoría, validaciones y lógica de negocio
/// basada en el usuario autenticado.
/// </summary>
/// 

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.Auditory
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.NameIdentifier);

                return userIdClaim != null
                    ? int.Parse(userIdClaim.Value)
                    : 0;
            }
        }

        public string Email
        {
            get
            {
                return _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.Email)?.Value ?? "";
            }
        }

        public string Role
        {
            get
            {
                return _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.Role)?.Value ?? "";
            }
        }
    }
}