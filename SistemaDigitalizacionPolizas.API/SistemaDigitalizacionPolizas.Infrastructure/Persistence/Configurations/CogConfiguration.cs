using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class CogConfiguration
        : IEntityTypeConfiguration<COG>
    {

        public void Configure(EntityTypeBuilder<COG> builder)
        {
            builder.ToTable("ClasificadorObjectoGasto");

            builder.HasKey(x => x.idCog);

            builder.Property(x => x.idCog)
                   .HasColumnName("idCog");

            builder.Property(x => x.Code)
                   .HasColumnName("clave");

            builder.Property(x => x.Description)
                   .HasColumnName("descripcion");
            builder.Property(x => x.Active)
                   .HasColumnName("Activo");
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
