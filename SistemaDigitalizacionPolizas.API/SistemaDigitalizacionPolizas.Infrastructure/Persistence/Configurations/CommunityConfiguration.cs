using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class CommunityConfiguration
        : IEntityTypeConfiguration<Community>
    {
        public void Configure(EntityTypeBuilder<Community> builder)
        {
            builder.ToTable("Comunidades");

            builder.HasKey(x => x.idCommunity);

            builder.Property(x => x.idCommunity)
                    .HasColumnName("idComunidad");

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
