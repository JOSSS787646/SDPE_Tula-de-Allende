using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.Acquisition_Entities;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class AcquisitionClassificationConfiguration
        : IEntityTypeConfiguration<AcquisitionClassification>
    {
        public void Configure(EntityTypeBuilder<AcquisitionClassification> builder)
        {
            builder.ToTable("clasificacionAdquisiciones");

            builder.HasKey(x => x.idAcquisitionClassification);

            builder.Property(x => x.idAcquisitionClassification)
                   .HasColumnName("idclasificacionAquisiciones");

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

            builder.HasMany(x => x.DocumentTypes)
       .WithOne(x => x.ClassificationAcquisition)
       .HasForeignKey(x => x.ClassificationAcquisitionId)
       .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
