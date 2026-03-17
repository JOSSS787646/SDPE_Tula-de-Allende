using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestStatusHistory_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class RequestStatusHistoryConfiguration : IEntityTypeConfiguration<RequestStatusHistory>
    {
        public void Configure(EntityTypeBuilder<RequestStatusHistory> builder)
        {
            builder.ToTable("SolicitudEstados");

            builder.HasKey(x => x.IdRequestStatus);

            builder.Property(x => x.IdRequestStatus)
                .HasColumnName("idSolicitudEstado");

            builder.Property(x => x.IdRequest)
                .HasColumnName("idSolictud");

            builder.Property(x => x.IdStatus)
                .HasColumnName("idEstadoSolicitud");

            builder.Property(x => x.StatusChangeDate)
                .HasColumnName("fechaCambio");

            builder.Property(x => x.Observations)
                .HasColumnName("obervaciones");

            builder.Property(x => x.ResponsibleUserId)
                .HasColumnName("usuarioResponsable");

            builder.Property(x => x.CreatedBy)
                .HasColumnName("creadoPor");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("fechaCreacion");

            builder.Property(x => x.UpdatedBy)
                .HasColumnName("modificadoPor");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("fechaModificacion");

            builder.Property(x => x.Active)
                .HasColumnName("activo");
        }
    }
}
