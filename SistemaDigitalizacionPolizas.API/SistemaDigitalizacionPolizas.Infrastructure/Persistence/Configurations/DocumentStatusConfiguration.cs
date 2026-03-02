using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.ApplicationStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class DocumentStatusConfiguration
         : IEntityTypeConfiguration<DocumentStatus>
    {
        public void Configure(EntityTypeBuilder<DocumentStatus> builder)
        {
            // 🔹 Nombre de tabla
            builder.ToTable("EstadoDocumento");

            // 🔹 Primary Key
            builder.HasKey(x => x.idDocumentStatus);

            builder.Property(x => x.idDocumentStatus)
                   .HasColumnName("idEstadoDocumento")
                   .ValueGeneratedOnAdd();

            // 🔹 Business fields
            builder.Property(x => x.Code)
                   .HasColumnName("clave")
                   .IsRequired();

            builder.Property(x => x.Description)
                   .HasColumnName("descripcion")
                   .HasMaxLength(45)
                   .IsRequired();

            builder.Property(x => x.Order)
                   .HasColumnName("orden")
                   .IsRequired();

            builder.Property(x => x.Active)
                   .HasColumnName("activo")
                   .HasDefaultValue(true);

            // 🔹 Auditoría
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
