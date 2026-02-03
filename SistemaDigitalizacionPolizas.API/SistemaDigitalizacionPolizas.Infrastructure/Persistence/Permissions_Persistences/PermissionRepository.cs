using Microsoft.Data.SqlClient;
using SistemaDigitalizacionPolizas.Domain.Dtos.Permission;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Permissions;
using System.Data;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Permissions_Persistences
{
    internal class PermissionRepository : IPermissionRepository
    {
        private readonly SdpeDbContext _context;

        public PermissionRepository(SdpeDbContext context)
        {
            _context = context;
        }

        public async Task<List<PermissionDto>> GetPermissionByRole(int idRol)
        {
            var data = await _context.Set<PermissionQueryResult>()
                .FromSqlRaw(
                    "EXEC sp_ObtenerPermisosPorRol @idRol",
                    new SqlParameter("@idRol", idRol)
                )
                .ToListAsync();

            // 🔥 MAPEO CORRECTO
            return data.Select(p => new PermissionDto
            {
                IdPermiso = p.IdPermiso,
                Modulo = p.Modulo ?? string.Empty,
                Accion = p.Accion ?? string.Empty,
                Asignado = p.Asignado
            }).ToList();

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
