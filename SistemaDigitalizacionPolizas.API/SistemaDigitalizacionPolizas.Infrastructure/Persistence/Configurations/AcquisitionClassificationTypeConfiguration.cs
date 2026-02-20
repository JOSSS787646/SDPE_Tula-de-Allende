using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class AcquisitionClassificationTypeConfiguration
     : IEntityTypeConfiguration<AcquisitionClassificationType>
    {
        public void Configure(EntityTypeBuilder<AcquisitionClassificationType> builder)
        {
            // ===============================
            // Tabla
            // ===============================
            builder.ToTable("TipoClasificacionAdquisicion");

            // ===============================
            // Primary Key
            // ===============================
            builder.HasKey(x => x.IdAcquisitionClassificationType);

            builder.Property(x => x.IdAcquisitionClassificationType)
                .HasColumnName("idTipoClasificacion")
                .ValueGeneratedOnAdd();

            // ===============================
            // Foreign Keys
            // ===============================
            builder.Property(x => x.IdAcquisitionType)
                .HasColumnName("idTipoAdquisicion")
                .IsRequired(false);

            builder.Property(x => x.IdClassificationAcquisition)
                .HasColumnName("idClasificacionAdquisicion")
                .IsRequired(false);

            // ===============================
            // Auditoría
            // ===============================
            builder.Property(x => x.CreatedBy)
                .HasColumnName("creadoPor")
                .HasDefaultValue(1);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("fechaCreacion")
                .HasDefaultValueSql("SYSDATETIME()");

            builder.Property(x => x.ModifiedBy)
                .HasColumnName("modificadoPor");

            builder.Property(x => x.ModifiedAt)
                .HasColumnName("fechaModificacion");

            builder.Property(x => x.Active)
                .HasColumnName("activo")
                .HasDefaultValue(true);

            // ===============================
            // Relaciones
            // ===============================
            builder.HasOne(x => x.AcquisitionType)
                .WithMany()
                .HasForeignKey(x => x.IdAcquisitionType)
                .HasConstraintName("FK_TipoClasificacion_Tipo");

            builder.HasOne(x => x.AcquisitionClassification)
                .WithMany()
                .HasForeignKey(x => x.IdClassificationAcquisition)
                .HasConstraintName("FK_TipoClasificacion_Clasificacion");

            // ===============================
            // Índice único recomendado
            // ===============================
            builder.HasIndex(x => new
            {
                x.IdAcquisitionType,
                x.IdClassificationAcquisition
            })
            .IsUnique();
        }
    }
}
