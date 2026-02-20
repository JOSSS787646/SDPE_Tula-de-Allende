using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class ClasificationDocumentTypeConfiguration
        : IEntityTypeConfiguration<ClasificationDocumentType>
    {
        public void Configure(EntityTypeBuilder<ClasificationDocumentType> builder)
        {
            // =====================================
            // Table
            // =====================================
            builder.ToTable("TipoClasificacionDocumento");

            // =====================================
            // Primary Key
            // =====================================
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            // =====================================
            // Foreign Keys
            // =====================================
            builder.Property(x => x.ClassificationAcquisitionId)
                .HasColumnName("idClasificacionAdquisicion")
                .IsRequired();

            builder.Property(x => x.DocumentTypeId)
                .HasColumnName("idTipoDocumento")
                .IsRequired();

            // =====================================
            // Business Fields
            // =====================================
            builder.Property(x => x.IsRequired)
                .HasColumnName("obligatorio")
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(x => x.Active)
                .HasColumnName("activo")
                .HasDefaultValue(true)
                .IsRequired();

            // =====================================
            // Relationships
            // =====================================

            builder.HasOne(x => x.ClassificationAcquisition)
    .WithMany(x => x.DocumentTypes)
    .HasForeignKey(x => x.ClassificationAcquisitionId)
    .HasPrincipalKey(x => x.idAcquisitionClassification)
    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.DocumentType)
                .WithMany(x => x.Classifications)
                .HasForeignKey(x => x.DocumentTypeId)
                .HasPrincipalKey(x => x.IdDocumentType)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================
            // Unique Index (avoid duplicates)
            // =====================================
            builder.HasIndex(x => new
            {
                x.ClassificationAcquisitionId,
                x.DocumentTypeId
            })
            .IsUnique();
        }
    }
}
