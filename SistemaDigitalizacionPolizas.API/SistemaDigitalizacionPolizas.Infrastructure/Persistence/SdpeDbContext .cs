using SistemaDigitalizacionPolizas.Domain.Entities.Acquisition_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.Actions_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.ApplicationStatus;
using SistemaDigitalizacionPolizas.Domain.Entities.Auth_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.Supplier_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences;

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
        public DbSet<COG> Cog { get; set; }
        public DbSet<FundingSource> FundingSources { get; set; }
        public DbSet<Proyect> Proyects { get; set; }
        public DbSet<Prog> Progs { get; set; }
        public DbSet <ActionsPolicy> Actions { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<AcquisitionType> AcquisitionTypes { get; set; }
        public DbSet<AcquisitionClassification> AcquisitionClassifications { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<DocumentType> Documents { get; set; }
        public DbSet<ClasificationDocumentType> ClasificationDocumentTypes { get; set; }
        public DbSet<ExpedientDocument> ExpedientDocuments { get; set; }
        public DbSet<ApplicationStatus> ApplicationStatuses { get; set; }
        public DbSet<DocumentStatus> DocumentStatuses { get; set; }


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
