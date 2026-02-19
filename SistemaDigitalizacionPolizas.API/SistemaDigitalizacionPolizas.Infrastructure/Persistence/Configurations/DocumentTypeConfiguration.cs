using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class DocumentTypeConfiguration
     : IEntityTypeConfiguration<DocumentType>
    {
        public void Configure(EntityTypeBuilder<DocumentType> builder)
        {
            // =====================================
            // Table
            // =====================================
            builder.ToTable("TipoDocumento");

            // =====================================
            // Primary Key
            // =====================================
            builder.HasKey(x => x.IdDocumentType);

            builder.Property(x => x.IdDocumentType)
                .HasColumnName("idTipoDocumento")
                .ValueGeneratedOnAdd();

            // =====================================
            // Business Fields
            // =====================================
            builder.Property(x => x.DocumentName)
                .HasColumnName("nombreDocumento")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("descripcion")
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(x => x.IsRequired)
                .HasColumnName("obligatorio")
                .IsRequired();

            // =====================================
            // Audit Fields
            // =====================================
            builder.Property(x => x.CreatedBy)
                .HasColumnName("creadoPor")
                .HasDefaultValue(1);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("fechaCreacion")
                .HasDefaultValueSql("SYSDATETIME()");

            builder.Property(x => x.ModifiedBy)
                .HasColumnName("modificadoPor")
                .IsRequired(false);

            builder.Property(x => x.ModifiedAt)
                .HasColumnName("fechaModificacion")
                .IsRequired(false);

            // =====================================
            // Soft Delete
            // =====================================
            builder.Property(x => x.Active)
                .HasColumnName("activo")
                .HasDefaultValue(true);
        }
    }
}