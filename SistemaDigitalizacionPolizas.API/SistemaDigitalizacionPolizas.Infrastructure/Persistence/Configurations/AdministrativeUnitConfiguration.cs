using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class AdministrativeUnitConfiguration
    : IEntityTypeConfiguration<AdministrativeUnit>
    {
        public void Configure(EntityTypeBuilder<AdministrativeUnit> builder)
        {
            builder.ToTable("UnidadesAdministrativas");

            builder.HasKey(x => x.IdAdministrativeUnit);

            builder.Property(x => x.IdAdministrativeUnit)
                   .HasColumnName("idUnidadesAdministrativas");

            builder.Property(x => x.Code)
                   .HasColumnName("clave");

            builder.Property(x => x.Description)
                   .HasColumnName("descripcion");
        }
    }
}
