using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.Acquisition_Entities;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class AcquisitionTypeConfiguration
        : IEntityTypeConfiguration<AcquisitionType>
    {
        public void Configure(EntityTypeBuilder<AcquisitionType> builder)
        {
            builder.ToTable("TipoAdquiciones");

            builder.HasKey(x => x.idAcquisitionType);

            builder.Property(x => x.idAcquisitionType)
                   .HasColumnName("idTipoAquisiciones");

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
