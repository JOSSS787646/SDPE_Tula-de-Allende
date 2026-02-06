using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaDigitalizacionPolizas.Domain.Entities.Actions_Entities;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class ActionConfiguration
        :IEntityTypeConfiguration<Domain.Entities.Actions_Entities.ActionsPolicy>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Actions_Entities.ActionsPolicy> builder)
        {
            builder.ToTable("Acciones");

            builder.HasKey(x => x.IdAction);

            builder.Property(x => x.IdAction)
                   .HasColumnName("idAcciones");

            builder.Property(x => x.Code)
                   .HasColumnName("clave");

            builder.Property(x => x.Description)
                   .HasColumnName("descripcion");
            builder.Property(x => x.Active)
                   .HasColumnName("activo");
            builder.Property(x => x.CreatedBy)
                 .HasColumnName("creadoPor");
            builder.Property(x => x.CreatedAt)
                   .HasColumnName("fechaCreacion");
            builder.Property(x => x.UpdatedBy)
                   .HasColumnName("modificadoPor");
            builder.Property(x => x.UpdatedAt)
                   .HasColumnName("fechaModificacion");
        }
    }
}
