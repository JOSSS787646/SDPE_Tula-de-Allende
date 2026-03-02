using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class RequestDocumentExceptionConfiguration
       : IEntityTypeConfiguration<RequestDocumentException>
    {
        public void Configure(EntityTypeBuilder<RequestDocumentException> builder)
        {
            builder.ToTable("SolicitudDocumentoExcepcion");

            // 🔹 Primary Key
            builder.HasKey(x => x.IdRequestDocumentException);

            builder.Property(x => x.IdRequestDocumentException)
                   .HasColumnName("idSolicitudDocumentoExcepcion");

            // 🔹 Foreign Keys
            builder.Property(x => x.IdRequest)
                   .HasColumnName("idSolicitud")
                   .IsRequired();

            builder.Property(x => x.IdDocumentType)
                   .HasColumnName("idTipoDocumento")
                   .IsRequired();

            // 🔹 Business Fields
            builder.Property(x => x.DoesNotApply)
                   .HasColumnName("noAplica")
                   .IsRequired();

            builder.Property(x => x.Justification)
                   .HasColumnName("justificacion")
                   .HasMaxLength(500)
                   .IsRequired();

            // 🔹 Audit Fields
            builder.Property(x => x.CreatedBy)
                   .HasColumnName("creadoPor");

            builder.Property(x => x.CreatedAt)
                   .HasColumnName("fechaCreacion")
                   .HasDefaultValueSql("SYSDATETIME()");

            // 🔹 Soft Delete
            builder.Property(x => x.Active)
                   .HasColumnName("activo")
                   .HasDefaultValue(true);

            // 🔹 Unique Constraint (Regla de negocio de BD)
            builder.HasIndex(x => new { x.IdRequest, x.IdDocumentType })
                   .IsUnique()
                   .HasDatabaseName("UQ_Solicitud_Documento");

            // 🔹 Relaciones (opcional)

            builder.HasOne(x => x.DocumentType)
               .WithMany(x => x.RequestDocumentExceptions)
               .HasForeignKey(x => x.IdDocumentType)
               .HasConstraintName("FK_SolicitudDocumentoExcepcion_TipoDocumento")
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Request)
                   .WithMany(x => x.DocumentExceptions)
                   .HasForeignKey(x => x.IdRequest)
                   .HasConstraintName("FK_SolicitudDocumentoExcepcion_Solicitud")
                   .OnDelete(DeleteBehavior.Restrict);

        }


    }
}
