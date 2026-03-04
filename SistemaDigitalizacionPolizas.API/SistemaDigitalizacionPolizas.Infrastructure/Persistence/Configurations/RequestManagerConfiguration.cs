using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestManager_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class RequestManagerConfiguration
       : IEntityTypeConfiguration<RequestManager>
    {
        public void Configure(EntityTypeBuilder<RequestManager> builder)
        {
            builder.ToTable("Encargados");

            builder.HasKey(x => x.IdRequestManager);

            builder.Property(x => x.IdRequestManager)
                .HasColumnName("idEncargado");

            builder.Property(x => x.IdRequest)
                .HasColumnName("idSolicitud")
                .IsRequired();

            builder.Property(x => x.IdAdministrativeUnit)
                .HasColumnName("idUnidadAdministrativa");

            builder.Property(x => x.FirstName)
                .HasColumnName("nombre")
                .HasMaxLength(65)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasColumnName("apellidoPaterno")
                .HasMaxLength(65)
                .IsRequired();

            builder.Property(x => x.SecondLastName)
                .HasColumnName("apellidoMaterno")
                .HasMaxLength(65);

            builder.Property(x => x.Email)
                .HasColumnName("correo")
                .HasMaxLength(100);

            builder.Property(x => x.Phone)
                .HasColumnName("telefono")
                .HasMaxLength(20);

            builder.HasOne(x => x.Request)
                .WithMany(x => x.RequestManagers)
                .HasForeignKey(x => x.IdRequest)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AdministrativeUnit)
                .WithMany()
                .HasForeignKey(x => x.IdAdministrativeUnit)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
