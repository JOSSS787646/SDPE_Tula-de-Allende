using Microsoft.Data.SqlClient;
using SistemaDigitalizacionPolizas.Domain.Dtos.Permission;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Permissions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Permissions_Persistences
{
    internal class PermissionRepository: IPermissionRepository
    {
        private readonly SdpeDbContext _context;

        public PermissionRepository(SdpeDbContext context)
        {
            _context = context;
        }

        public async Task<List<PermissionDto>> GetPermissionByRole(int idRol)
        {
            return await _context.Set<PermissionDto>()
                .FromSqlRaw("EXEC sp_ObtenerPermisosPorRol @idRol",
                    new SqlParameter("@idRol", idRol))
                .ToListAsync();
        }

        public async Task UpdatePermissionByRole(PermissionUpdateRoleDto dto)
        {
            // Crear DataTable para TVP
            var table = new DataTable();
            table.Columns.Add("idPermiso", typeof(int));

            foreach (var id in dto.Permisos)
                table.Rows.Add(id);

            var paramRol = new SqlParameter("@idRol", dto.IdRol);
            var paramPermisos = new SqlParameter("@Permisos", table)
            {
                TypeName = "dbo.PermisoTableType",
                SqlDbType = SqlDbType.Structured
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_ActualizarPermisosRol @idRol, @Permisos",
                paramRol, paramPermisos
            );
        }
    }
}
