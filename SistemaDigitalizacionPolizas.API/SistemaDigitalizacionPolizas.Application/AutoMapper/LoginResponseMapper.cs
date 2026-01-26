
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.AutoMapper
{
    public static class LoginResponseMapper
    {
        public static LoginResponse Map(User user, JwtResult jwt)
        {
            return new LoginResponse
            {
                Token = jwt.Token,
                Expiration = jwt.ExpiresAt,

                User = new UserSessionDto
                {
                    IdUser = user.IdUser,
                    Email = user.Email,
                    Role = user.Role.RolName,
                    AdministrativeUnit = user.AdministrativeUnit.Description
                },

                Permissions = user.Role.PermissionRoles
                    .GroupBy(p => p.Permission.Module)
                    .Select(g => new PermissionGroupDto
                    {
                        Module = g.Key,
                        Action = g.Select(x => x.Permission.Action).ToList()
                    })
                    .ToList()
            };
        }
    }
}
