using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class ExpedientDocumentConfiguration
       : IEntityTypeConfiguration<ExpedientDocument>
    {
        public void Configure(EntityTypeBuilder<ExpedientDocument> builder)
        {
            // ============================================
            // Tabla
            // ============================================
            builder.ToTable("DocumentoExpediente");

            // ============================================
            // Primary Key
            // ============================================
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("idDocumentoExpediente")
                .ValueGeneratedOnAdd();

            // ============================================
            // Foreign Keys
            // ============================================
            builder.Property(x => x.RequestId)
                .HasColumnName("idSolicitud");

            builder.Property(x => x.DocumentTypeId)
                .HasColumnName("idTipoDocumento");

            // ============================================
            // Información del Archivo
            // ============================================
            builder.Property(x => x.FileName)
                .HasColumnName("nombreArchivo")
                .HasMaxLength(65);

            builder.Property(x => x.FilePath)
                .HasColumnName("ruraArchivo")
                .HasMaxLength(250);

            builder.Property(x => x.UploadDate)
                .HasColumnName("fechaCarga")
                .HasColumnType("date");

            // ============================================
            // Estado de negocio
            // ============================================
            builder.Property(x => x.DocumentStatus)
                .HasColumnName("estatusDocumento")
                .HasMaxLength(45);

            builder.Property(x => x.Observations)
                .HasColumnName("observaciones")
                .HasMaxLength(45);

            // ============================================
            // Auditoría
            // ============================================
            builder.Property(x => x.UploadedBy)
                .HasColumnName("cargadoPor");

            builder.Property(x => x.CreatedBy)
                .HasColumnName("creadoPor")
                .HasDefaultValue(1);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("fechaCreacion")
                .HasColumnType("datetime2")
                .HasDefaultValueSql("SYSDATETIME()");

            builder.Property(x => x.ModifiedBy)
                .HasColumnName("modificadoPor");

            builder.Property(x => x.ModifiedAt)
                .HasColumnName("fechaModificacion")
                .HasColumnType("datetime2");

            // ============================================
            // Soft Delete
            // ============================================
            builder.Property(x => x.Active)
                .HasColumnName("activo")
                .HasDefaultValue(true);


            // ============================================
            // Relationship with DocumentType
            // ============================================

            builder.HasOne(x => x.DocumentType)
                .WithMany(x => x.ExpedientDocuments)
                .HasForeignKey(x => x.DocumentTypeId)
                .HasConstraintName("FK_Documento_Tipo")
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
