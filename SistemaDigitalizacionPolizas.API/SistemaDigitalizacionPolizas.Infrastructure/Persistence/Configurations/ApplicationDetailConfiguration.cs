using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class ApplicationDetailConfiguration : IEntityTypeConfiguration<ApplicationDetail>
    {
        public void Configure(EntityTypeBuilder<ApplicationDetail> builder)
        {
            builder.ToTable("DetallesSolicitud");

            builder.HasKey(x => x.IdDetail);

            builder.Property(x => x.IdDetail)
                .HasColumnName("idDetalle");

            builder.Property(x => x.ApplicationId)
                .HasColumnName("idSolicitud")
                .IsRequired();

            builder.Property(x => x.CogId)
                .HasColumnName("idCog")
                .IsRequired();

            builder.Property(x => x.Quantity)
                .HasColumnName("cantidad")
                .IsRequired();

            builder.Property(x => x.UnitMeasure)
                .HasColumnName("unidadMedida")
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasColumnName("descripcion")
                .HasMaxLength(500);

            builder.Property(x => x.UnitAmount)
                .HasColumnName("importeUnitario")
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TotalAmount)
                .HasColumnName("importeTotal")
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.CreatedBy)
                .HasColumnName("creadoPor");

            builder.Property(x => x.CreatedDate)
                .HasColumnName("fechaCreacion");

            builder.Property(x => x.ModifiedBy)
                .HasColumnName("modificadoPor");

            builder.Property(x => x.ModifiedDate)
                .HasColumnName("fechaModificacion");

            builder.Property(x => x.Active)
                .HasColumnName("activo");

            // 🔗 RELATIONSHIP
            builder.HasOne(x => x.AcquisitionRequest)
                .WithMany(a => a.Details)
                .HasForeignKey(x => x.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Cog)
             .WithMany(c => c.Details)
     .HasForeignKey(x => x.CogId)
     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
