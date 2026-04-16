using SistemaDigitalizacionPolizas.Domain.Entities.Auth_Entities;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Auth_Persistences
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly SdpeDbContext _context;

        public PasswordResetRepository(SdpeDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(PasswordResetToken token)
        {
            await _context.PasswordResetTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        //Valida el token de recuperación de contraseña
        public async Task<PasswordResetToken?> GetValidTokenAsync(int userId, string code)
        {
            return await _context.PasswordResetTokens
                .FirstOrDefaultAsync(x =>
                    x.IdUser == userId &&
                    x.Code == code &&
                    !x.Used &&
                    x.ExpirationDate > DateTime.UtcNow
                );
        }

        public async Task InvalidateAsync(int id)
        {
            var token = await _context.PasswordResetTokens.FindAsync(id);

            if (token != null)
            {
                token.Used = true;
                await _context.SaveChangesAsync();
            }
        }



    }
}
