using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.Supplier_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Proveedores");

            builder.HasKey(x => x.IdSupplier);

            builder.Property(x => x.IdSupplier)
                   .HasColumnName("idProveedores");

            builder.Property(x => x.Rfc)
                   .HasColumnName("RFC");

            builder.Property(x => x.BusinessName)
                   .HasColumnName("razonSocial")
                   .HasMaxLength(60)
                   .IsRequired();

            builder.Property(x => x.Street)
                   .HasColumnName("calle")
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(x => x.ExternalNumber)
                   .HasColumnName("numeroExterior")
                   .HasMaxLength(10);

            builder.Property(x => x.InternalNumber)
                   .HasColumnName("numeroInterior")
                   .HasMaxLength(10);

            builder.Property(x => x.Neighborhood)
                   .HasColumnName("colonia")
                   .HasMaxLength(100);

            builder.Property(x => x.PostalCode)
                   .HasColumnName("codigo postal");

            builder.Property(x => x.City)
                   .HasColumnName("ciudad")
                   .HasMaxLength(45);

            builder.Property(x => x.Municipality) 
                   .HasColumnName("municipio")
                   .HasMaxLength(45)
                   .IsRequired();

            builder.Property(x => x.State)
                   .HasColumnName("estado")
                   .HasMaxLength(45)
                   .IsRequired();

            builder.Property(x => x.Country)
                   .HasColumnName("pais")
                   .HasMaxLength(45)
                   .IsRequired();

            builder.Property(x => x.Phone)
                   .HasColumnName("telefono")
                   .HasMaxLength(20);

            builder.Property(x => x.ContactName)
                   .HasColumnName("nombreContacto")
                   .HasMaxLength(45);

            builder.Property(x => x.ContactPhone)
                   .HasColumnName("telefonoContacto")
                   .HasMaxLength(20);

            builder.Property(x => x.Email)
                   .HasColumnName("correo")
                   .HasMaxLength(100);

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
