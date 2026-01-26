

using SistemaDigitalizacionPolizas.Domain.Dtos.Permission;
using SistemaDigitalizacionPolizas.Domain.Dtos.User;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        public UserSessionDto User { get; set; }
        public List<PermissionGroupDto> Permissions { get; set; }
    }
}
