using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.PaymentPolicy_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class PaymentPolicyConfiguration : IEntityTypeConfiguration<PaymentPolicy>
    {
        public void Configure(EntityTypeBuilder<PaymentPolicy> builder)
        {
            builder.ToTable("PolizaPago");

            builder.HasKey(x => x.IdPaymentPolicy);

            builder.Property(x => x.IdPaymentPolicy)
                .HasColumnName("idPolizaPago");

            builder.Property(x => x.PolicyCode)
                .HasColumnName("codigoPoliza")
                .HasMaxLength(65)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("descripcion")
                .HasMaxLength(500);

            builder.Property(x => x.FilePath)
                .HasColumnName("rutaArchivo")
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .HasColumnName("creadoPor");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("fechaCreacion");

            builder.Property(x => x.IsActive)
                .HasColumnName("activo");

            builder.HasMany(x => x.Requests)
        .WithOne(x => x.PaymentPolicy)
        .HasForeignKey(x => x.IdPaymentPolicy)
        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
