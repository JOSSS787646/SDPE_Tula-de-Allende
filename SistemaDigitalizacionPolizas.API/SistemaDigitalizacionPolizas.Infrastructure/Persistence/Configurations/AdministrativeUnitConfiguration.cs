using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class AdministrativeUnitConfiguration
    : IEntityTypeConfiguration<AdministrativeUnit>
    {
        public void Configure(EntityTypeBuilder<AdministrativeUnit> builder)
        {
            builder.ToTable("UnidadesAdministrativas");

            builder.HasKey(x => x.IdAdministrativeUnit);

            builder.Property(x => x.IdAdministrativeUnit)
                   .HasColumnName("idUnidadesAdministrativas");

            builder.Property(x => x.Code)
                   .HasColumnName("clave");

            builder.Property(x => x.Description)
                   .HasColumnName("descripcion");

            builder.Property(x => x.Active)
            .HasColumnName("activo");
            builder.Property(x => x.CreatedBy)
                   .HasColumnName("creadoPor");
            builder.Property(x => x.CreatedAt)
                   .HasColumnName("fechaCreacion");
            builder.Property(x => x.UpdatedBy)
                   .HasColumnName("modificadoPor");
            builder.Property(x => x.UpdatedAt)
                   .HasColumnName("fechaModificacion");
        }
    }
}
