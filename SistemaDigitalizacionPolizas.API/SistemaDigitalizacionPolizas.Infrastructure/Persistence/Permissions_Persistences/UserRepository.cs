


using SistemaDigitalizacionPolizas.Domain.Dtos.User;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Permissions_Persistences
{
    public class UserRepository : IUserRepository
    {
        private readonly SdpeDbContext _context;

        public UserRepository(SdpeDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetUserIdByRoleAsync(int roleId)
        {
            var user = await _context.Users
                .Where(u => u.IdRole == roleId && u.Asset)
                .Select(u => new { u.IdUser })
                .FirstOrDefaultAsync();

            return user?.IdUser ?? 0;
        }


        //Obtiene a un usuario por su gamil
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task<string?> GetEmailByRoleAsync(int roleId)
        {
            return await _context.Users
                .Where(x => x.IdRole == roleId && x.Asset)
                .Select(x => x.Email)
                .FirstOrDefaultAsync();
        }
        //Obtiene a un usurio con su sul, area, token y permisos
        public async Task<User?> GetUserWithRolesAndPermissionsAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.PermissionRoles)
                        .ThenInclude(pr => pr.Permission)
                .Include(u => u.AdministrativeUnit)
                .FirstOrDefaultAsync(u => u.Email == email);
        }


        //Actuliza el ultimo acceso del usuario al sistema
        public async Task UpdateLastAccessAsync(int idUser)
        {
            var user = await _context.Users.FindAsync(idUser);
            if (user != null)
            {
                user.LastAccess = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        //CREA A UN NUEVO USUARIO
        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        //Actualiza la contraseña de un usuario
        public async Task<bool> UpdatePasswordAsync(int idUser, string hashedPassword)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.IdUser == idUser);

            if (user == null)
                return false;

            user.Password = hashedPassword;
            user.UpdateDate = DateTime.Now;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        //Obtiene todos los usuarios con el nomnre de rol y unidad administrativa
        public async Task<List<UserListDto>> GetUsersAsync(int page, int pageSize)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.Role)
                .Include(u => u.AdministrativeUnit)
                .OrderBy(u => u.IdUser)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserListDto
                {
                    IdUser = u.IdUser,
                    Email = u.Email,
                    Role = u.Role.RolName,
                    idRole=u.IdRole,
                    AdministrativeUnit = u.AdministrativeUnit.Description,
                    IdAdministrativeUnit=u.IdAdministrativeUnit,
                    Asset = u.Asset
                })
                .ToListAsync();
        }


        //Obtiene un usuario por su ID
        public async Task<User?> GetByIdAsync(int idUser)
        {
            return await _context.Users
                .FirstOrDefaultAsync(r => r.IdUser == idUser);
        }


        public async Task<bool> UpdateStatusAsync(int idUser, bool status)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.IdUser == idUser);

            if (user == null)
                return false;

            user.Asset = status;
            user.UpdateDate = DateTime.Now;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        //Actualiza los datos del usuario
        public async Task<bool> UpdateUserDataAsync(
        int idUser,
        string email,
        int idRole,
        int idAdministrativeUnit
)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.IdUser == idUser);

            if (user == null)
                return false;

            user.Email = email;
            user.IdRole = idRole;
            user.IdAdministrativeUnit = idAdministrativeUnit;
            user.UpdateDate = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
