using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notificaciones");

            builder.HasKey(x => x.IdNotification);

            builder.Property(x => x.IdNotification)
                .HasColumnName("IdNotificacion");

            builder.Property(x => x.UserId)
                .HasColumnName("IdUsuario");

            builder.Property(x => x.Title)
                .HasColumnName("Titulo")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Message)
                .HasColumnName("Mensaje")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.RequestId)
                .HasColumnName("IdSolicitud");

            builder.Property(x => x.IsRead)
                .HasColumnName("Leida")
                .HasDefaultValue(false);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("FechaCreacion")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.CreatedBy)
                .HasColumnName("CreadoPor");

            builder.Property(x => x.Active)
                .HasColumnName("Activo")
                .HasDefaultValue(true);
        }
    }
}
