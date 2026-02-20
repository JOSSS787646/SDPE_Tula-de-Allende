using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    internal class BeneficiaryConfiguration
        : IEntityTypeConfiguration<Beneficiary>
    {
        public void Configure(EntityTypeBuilder<Beneficiary> builder)
        {
            // ===============================
            // Tabla
            // ===============================
            builder.ToTable("Beneficiarios");

            // ===============================
            // Clave primaria
            // ===============================
            builder.HasKey(b => b.IdBeneficiary);

            builder.Property(b => b.IdBeneficiary)
                .HasColumnName("idBeneficiario")
                .ValueGeneratedOnAdd();

            // ===============================
            // Datos personales
            // ===============================
            builder.Property(b => b.FirstName)
                .HasColumnName("nombre")
                .HasMaxLength(65)
                .IsRequired();

            builder.Property(b => b.PaternalLastName)
                .HasColumnName("apellidoPat")
                .HasMaxLength(65)
                .IsRequired();

            builder.Property(b => b.MaternalLastName)
                .HasColumnName("apellidoMat")
                .HasMaxLength(65)
                .IsRequired(false);

            // ===============================
            // Dirección
            // ===============================
            builder.Property(b => b.Street)
                .HasColumnName("calle")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(b => b.ExternalNumber)
                .HasColumnName("numeroExterior")
                .HasMaxLength(10)
                .IsRequired(false);

            builder.Property(b => b.InternalNumber)
                .HasColumnName("numeroInterior")
                .HasMaxLength(10)
                .IsRequired(false);

            builder.Property(b => b.Neighborhood)
                .HasColumnName("colonia")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(b => b.PostalCode)
                .HasColumnName("codigoPostal")
                .IsRequired();

            builder.Property(b => b.City)
                .HasColumnName("ciudad")
                .HasMaxLength(45)
                .IsRequired(false);

            builder.Property(b => b.Municipality)
                .HasColumnName("municipio")
                .HasMaxLength(45)
                .IsRequired();

            builder.Property(b => b.State)
                .HasColumnName("estado")
                .HasMaxLength(45)
                .IsRequired();

            builder.Property(b => b.Country)
                .HasColumnName("pais")
                .HasMaxLength(45)
                .IsRequired();

            // ===============================
            // Identificación
            // ===============================
            builder.Property(b => b.Ine)
                .HasColumnName("ine")
                .HasMaxLength(13)
                .IsRequired();

            builder.Property(b => b.Curp)
                .HasColumnName("curp")
                .HasMaxLength(18)
                .IsRequired();

            builder.Property(x => x.Phone)
             .HasColumnName("telefono")
             .HasMaxLength(20);

            builder.Property(b => b.Email)
                .HasColumnName("correo")
                .HasMaxLength(45)
                .IsRequired(false);

            // ===============================
            // Estado
            // ===============================
            builder.Property(b => b.Active)
                .HasColumnName("active")
                .HasDefaultValue(true)
                .IsRequired();

            // ===============================
            // Auditoría
            // ===============================
            builder.Property(b => b.CreatedBy)
                .HasColumnName("creadoPor")
                .HasDefaultValue(1)
                .IsRequired();

            builder.Property(b => b.CreatedAt)
                .HasColumnName("fechaCreacion")
                .HasDefaultValueSql("SYSDATETIME()")
                .IsRequired();

            builder.Property(b => b.ModifiedBy)
                .HasColumnName("modificadoPor")
                .IsRequired(false);

            builder.Property(b => b.ModifiedAt)
                .HasColumnName("fechaModificacion")
                .IsRequired(false);

            builder.Property(b => b.idCommunity)
            .HasColumnName("idComunidad")
             .IsRequired(false);


            // ===============================
            // Relación con Comunidad (Opcional)
            // ===============================
            builder.HasOne(b => b.Community)
                   .WithMany() // una comunidad puede tener muchos beneficiarios
                   .HasForeignKey(b => b.idCommunity)
                   .HasConstraintName("FK_Beneficiarios_Comunidad")
                   .OnDelete(DeleteBehavior.SetNull) // si borran comunidad → null
                   .IsRequired(false);

        }
    }
}
