using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.Auth_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class PasswordResetTokenConfiguration
       : IEntityTypeConfiguration<PasswordResetToken>
    {
        public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
        {
            // Tabla
            builder.ToTable("RecuperarContrasenia");

            // PK
            builder.HasKey(x => x.IdToken);

            builder.Property(x => x.IdToken)
                   .HasColumnName("Id")
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.IdUser)
                   .HasColumnName("IdUsuario")
                   .IsRequired();

            builder.Property(x => x.Code)
                   .HasColumnName("Codigo")
                   .HasMaxLength(6)
                   .IsRequired();

            builder.Property(x => x.ExpirationDate)
                   .HasColumnName("FechaExpiracion")
                   .IsRequired();

            builder.Property(x => x.Used)
                   .HasColumnName("Usado")
                   .HasDefaultValue(false);

            builder.Property(x => x.DateCreate)
                   .HasColumnName("FechaCreacion")
                   .HasDefaultValueSql("GETDATE()");

            // Relación
            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.IdUser)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
