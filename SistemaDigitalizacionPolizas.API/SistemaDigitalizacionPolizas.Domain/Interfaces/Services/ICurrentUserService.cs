using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Services
{
    public interface ICurrentUserService
    {
        int UserId { get; }

        string Email { get; }

        string Role { get; }
    }
}
