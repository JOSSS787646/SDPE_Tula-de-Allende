using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class CogConfiguration
        : IEntityTypeConfiguration<COG>
    {

        public void Configure(EntityTypeBuilder<COG> builder)
        {
            builder.ToTable("ClasificadorObjetoGasto");

            builder.HasKey(x => x.idCog);

            builder.Property(x => x.idCog)
                   .HasColumnName("idUnidadesAdministrativas");

            builder.Property(x => x.Code)
                   .HasColumnName("clave");

            builder.Property(x => x.Description)
                   .HasColumnName("descripcion");
        }
    }
}
