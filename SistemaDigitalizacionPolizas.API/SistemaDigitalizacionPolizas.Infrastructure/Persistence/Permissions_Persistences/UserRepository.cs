


namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Permissions_Persistences
{
    public class UserRepository : IUserRepository
    {
        private readonly SdpeDbContext _context;

        public UserRepository(SdpeDbContext context)
        {
            _context = context;
        }
        

        //Obtiene a un usuario por su gamil
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
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


        //Actuliza el ultimi acceso del usuario al sistema
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
    }
}
