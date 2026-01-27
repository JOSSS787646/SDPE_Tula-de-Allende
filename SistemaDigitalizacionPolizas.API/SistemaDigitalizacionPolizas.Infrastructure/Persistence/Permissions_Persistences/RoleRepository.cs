using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Permissions_Persistences
{
    public class RoleRepository: IRoleRepository
    {

        private readonly SdpeDbContext _context;

        public RoleRepository(SdpeDbContext context)
        {
            _context = context;
        }

        //Obtiene todos los roles
        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles
                .AsNoTracking()
                .ToListAsync();
        }
        //Obtiene un rol por su Nombre
        public async Task<Role?> GetByNameAsync(string RolName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.RolName == RolName);
        }

        //Obtiene un rol por su ID
        public async Task<Role?> GetByIdAsync(int idRol)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.IdRol == idRol);
        }
        //Agrega un rol
        public async Task AddAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }
        //Actualiza un rol
        public async Task<bool> UpdateAsync(Role role)
        {
            var exists = await _context.Roles
                .AnyAsync(r => r.IdRol == role.IdRol);

            if (!exists)
                return false;

            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return true;
        }
        //Elimina un rol
        public async Task<bool> DeleteAsync(int id)
        {
            var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.IdRol == id);

            if (role == null)
                return false;

            role.Asset = false;

            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
