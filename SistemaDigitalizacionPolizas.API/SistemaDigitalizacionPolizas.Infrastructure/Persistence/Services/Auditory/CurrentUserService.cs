using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;


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
    }
}
