using SistemaDigitalizacionPolizas.Domain.Entities.Auth_Entities;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Auth
{
    public interface IPasswordResetRepository
    {
        Task CreateAsync(PasswordResetToken token);
        Task<PasswordResetToken?> GetValidTokenAsync(int userId, string code);
        Task InvalidateAsync(int id);


    }
}
