using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class FundingSourceConfiguration
        : IEntityTypeConfiguration<FundingSource>
    {
        public void Configure(EntityTypeBuilder<FundingSource> builder)
        {
            builder.ToTable("Fondo");

            builder.HasKey(x => x.idFundingSource);

            builder.Property(x => x.idFundingSource)
                   .HasColumnName("idFondo");

            builder.Property(x => x.Code)
                   .HasColumnName("clave");

            builder.Property(x => x.Description)
                   .HasColumnName("descripcion");

            builder.Property(x => x.Active)
                   .HasColumnName("Activo");
        }
    }
}
