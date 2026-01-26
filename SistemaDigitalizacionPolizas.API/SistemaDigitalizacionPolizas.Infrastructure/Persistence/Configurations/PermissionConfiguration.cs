using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class PermissionConfiguration
     : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permisos");

            builder.HasKey(x => x.IdPermission);

            builder.Property(x => x.IdPermission)
                   .HasColumnName("idPermiso");

            builder.Property(x => x.PermissionName)
                   .HasColumnName("nombrePermiso")
                   .HasMaxLength(250)
                   .IsRequired();

            builder.Property(x => x.Module)
                   .HasColumnName("modulo")
                   .HasMaxLength(250)
                   .IsRequired();

            builder.Property(x => x.Action)
                   .HasColumnName("accion")
                   .HasMaxLength(250)
                   .IsRequired();

            builder.Property(x => x.Description)
                   .HasColumnName("descripcion")
                   .HasMaxLength(250);

            builder.HasMany(x => x.PermissionRoles)
                   .WithOne(x => x.Permission)
                   .HasForeignKey(x => x.IdPermission);
        }
    }
}
