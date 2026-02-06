using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class ProgConfiguration
        :IEntityTypeConfiguration<Prog>
    {
        public void Configure(EntityTypeBuilder<Prog> builder)
        {
            builder.ToTable("Prog");
            builder.HasKey(x => x.idProg);
            builder.Property(x => x.idProg)
                   .HasColumnName("idProg");
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
