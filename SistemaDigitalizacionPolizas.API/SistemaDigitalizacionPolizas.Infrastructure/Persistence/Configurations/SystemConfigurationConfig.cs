using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.SystemConfiguration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class SystemConfigurationConfig
       : IEntityTypeConfiguration<SystemConfiguration>
    {
        public void Configure(EntityTypeBuilder<SystemConfiguration> builder)
        {
            builder.ToTable("ConfiguracionSistema");

            builder.HasKey(x => x.IdConfiguration);

            builder.Property(x => x.IdConfiguration)
                .HasColumnName("IdConfiguracion");

            builder.Property(x => x.EmailsEnabled)
                .HasColumnName("CorreosActivos")
                .IsRequired();

            builder.Property(x => x.NotificationStartDate)
                .HasColumnName("FechaInicioNotificaciones");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("FechaCreacion")
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .HasColumnName("CreadoPor");

            builder.Property(x => x.Active)
                .HasColumnName("Activo")
                .IsRequired();
        }
    }
}
