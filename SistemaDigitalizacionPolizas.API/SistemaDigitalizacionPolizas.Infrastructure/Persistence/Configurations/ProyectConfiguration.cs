using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class ProyectConfiguration
        :IEntityTypeConfiguration<Proyect>
    {
        public void Configure(EntityTypeBuilder<Proyect> builder)
        {
            builder.ToTable("Proyecto");
            builder.HasKey(x => x.idProyect);
            builder.Property(x => x.idProyect)
                   .HasColumnName("idProyecto");
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
