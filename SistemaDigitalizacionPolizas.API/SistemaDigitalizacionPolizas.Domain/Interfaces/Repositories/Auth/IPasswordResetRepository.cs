using SistemaDigitalizacionPolizas.Domain.Entities.Auth_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Auth
{
    public interface IPasswordResetRepository
    {
        Task CreateAsync(PasswordResetToken token);
        Task<PasswordResetToken?> GetValidTokenAsync(int userId, string code);
        Task InvalidateAsync(int id);


    }
}
