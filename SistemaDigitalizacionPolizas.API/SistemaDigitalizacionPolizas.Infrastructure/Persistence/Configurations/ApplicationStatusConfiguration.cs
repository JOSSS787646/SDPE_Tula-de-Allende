using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.ApplicationStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class ApplicationStatusConfiguration
        : IEntityTypeConfiguration<ApplicationStatus>
    {
        public void Configure(EntityTypeBuilder<ApplicationStatus> builder)
        {
            // 🔹 Nombre de la tabla
            builder.ToTable("EstadoSolicitud");

            // 🔹 Primary Key
            builder.HasKey(x => x.IdApplicationStatus);

            builder.Property(x => x.IdApplicationStatus)
                   .HasColumnName("idEstadoSolicitud")
                   .ValueGeneratedOnAdd();

            // 🔹 Code
            builder.Property(x => x.Code)
                   .HasColumnName("clave")
                   .IsRequired();

            // 🔹 Description
            builder.Property(x => x.Description)
                   .HasColumnName("descripcion")
                   .HasMaxLength(45)
                   .IsRequired(false);

            // 🔹 Order
            builder.Property(x => x.Order)
       .HasColumnName("orden")
       .IsRequired();


            // 🔹 Active
            builder.Property(x => x.Active)
                   .HasColumnName("activo")
                   .HasDefaultValue(true);

            // 🔹 Audit Fields
            builder.Property(x => x.CreatedBy)
                   .HasColumnName("creadoPor")
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .HasColumnName("fechaCreacion")
                   .HasColumnType("datetime2")
                   .HasDefaultValueSql("sysdatetime()");

            builder.Property(x => x.UpdatedBy)
                   .HasColumnName("modificadoPor")
                   .IsRequired(false);

            builder.Property(x => x.UpdatedAt)
                   .HasColumnName("fechaModificacion")
                   .HasColumnType("datetime2")
                   .IsRequired(false);
        }
    }
}
