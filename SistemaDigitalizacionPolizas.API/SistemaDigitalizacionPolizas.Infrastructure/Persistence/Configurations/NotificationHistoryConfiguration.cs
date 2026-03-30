using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class NotificationHistoryConfiguration : IEntityTypeConfiguration<NotificationHistory>
    {
        public void Configure(EntityTypeBuilder<NotificationHistory> builder)
        {
            builder.ToTable("NotificacionesHistorial");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("IdHistorial");

            builder.Property(x => x.NotificationId)
                .HasColumnName("IdNotificacion");

            builder.Property(x => x.TargetUserId)
                .HasColumnName("IdUsuarioNotificacion");

            builder.Property(x => x.TargetUserName)
                .HasColumnName("NombreUsuario")
                .HasMaxLength(150);

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

            builder.Property(x => x.RequestNumber)
                .HasColumnName("NumeroSolicitud")
                .HasMaxLength(50);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("FechaCreacion");

            builder.Property(x => x.DeletedByUserId)
                .HasColumnName("IdUsuarioElimino")
                .IsRequired();

            builder.Property(x => x.DeletedAt)
                .HasColumnName("FechaEliminacion")
                .HasDefaultValueSql("SYSDATETIME()");

            builder.Property(x => x.Action)
                .HasColumnName("Accion")
                .HasMaxLength(50)
                .IsRequired();

            // 🔗 RELACIÓN SOLO CON USUARIO (IMPORTANTE)
            builder.HasOne(x => x.DeletedByUser)
                .WithMany()
                .HasForeignKey(x => x.DeletedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

    
        }
    }
    }
