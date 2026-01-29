

using SistemaDigitalizacionPolizas.Domain.Dtos.Permission;
using SistemaDigitalizacionPolizas.Domain.Entities.Auth_Entities;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence
{
    public class SdpeDbContext : DbContext
    {
        public SdpeDbContext(DbContextOptions<SdpeDbContext> options)
            : base(options) { }

        // Usuarios / Seguridad
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionRole> PermissionRoles { get; set; }

        //Recuperacion de Contraseñas
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }


        // Áreas
        public DbSet<AdministrativeUnit> AdministrativeUnits { get; set; }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(SdpeDbContext).Assembly
            );
            modelBuilder.Entity<PermissionQueryResult>()
             .HasNoKey()
             .ToView(null);
        }
    }
}
