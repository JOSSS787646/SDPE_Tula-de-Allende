using Azure.Core;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Configurations
{
    public class AcquisitionRequestConfiguration
      : IEntityTypeConfiguration<AcquisitionRequest>
    {
        public void Configure(EntityTypeBuilder<AcquisitionRequest> builder)
        {
            // =====================================================
            // TABLE
            // =====================================================

            builder.ToTable("Solicitud");

            // =====================================================
            // PRIMARY KEY
            // =====================================================

            builder.HasKey(x => x.IdRequest);

            builder.Property(x => x.IdRequest)
                   .HasColumnName("idSolictud") // ⚠ así está en la BD
                   .ValueGeneratedOnAdd();

            // =====================================================
            // BUSINESS FIELDS
            // =====================================================

            builder.Property(x => x.RequestNumber)
                   .HasColumnName("numeroSolicitud")
                   .HasMaxLength(25);

            builder.Property(x => x.RequestDate)
                   .HasColumnName("fechaSolicitud")
                   .HasColumnType("datetime");

            builder.Property(x => x.Justification)
                   .HasColumnName("justificacion")
                   .HasMaxLength(5000);



            builder.Property(x => x.CFDI)
                   .HasColumnName("cfdi")
                   .HasMaxLength(100);


            builder.Property(x => x.AuthorizationDate)
                   .HasColumnName("fechaAutorizacion")
                   .HasColumnType("date");

            builder.Property(x => x.Observations)
                   .HasColumnName("Obervaciones") // ⚠ error en BD
                   .HasMaxLength(1500);

            // =====================================================
            // FOREIGN KEYS
            // =====================================================

            builder.Property(x => x.IdAdministrativeUnit)
                   .HasColumnName("idUnidadAdministrativa");

            builder.Property(x => x.IdProject)
                   .HasColumnName("idProyecto");

            builder.Property(x => x.IdAcquisitionType)
                   .HasColumnName("idTipoAdquisicion");

            builder.Property(x => x.IdSupplier)
                   .HasColumnName("idProveedor");

            builder.Property(x => x.IdApplicationStatus)
                   .HasColumnName("idEstadoActual");

            builder.Property(x => x.IdFundingSource)
                   .HasColumnName("idFondo");

            builder.Property(x => x.IdAcquisitionClassification)
                   .HasColumnName("idClasificaionAdquisicion");

            builder.Property(x => x.IdProgram)
                   .HasColumnName("idProg");

            builder.Property(x => x.IdCommunity)
                   .HasColumnName("idComunidad");

            builder.Property(x => x.IdBeneficiary)
                   .HasColumnName("idBeneficiario");
            builder.Property(x => x.IdPaymentPolicy)
       .HasColumnName("idPolizaPago");

            // =====================================================
            // RELATIONSHIPS
            // =====================================================

            builder.HasOne(x => x.AdministrativeUnit)
                   .WithMany()
                   .HasForeignKey(x => x.IdAdministrativeUnit)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Project)
                   .WithMany()
                   .HasForeignKey(x => x.IdProject)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AcquisitionType)
                   .WithMany()
                   .HasForeignKey(x => x.IdAcquisitionType)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Supplier)
                   .WithMany()
                   .HasForeignKey(x => x.IdSupplier)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ApplicationStatus)
                   .WithMany()
                   .HasForeignKey(x => x.IdApplicationStatus)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.FundingSource)
                   .WithMany()
                   .HasForeignKey(x => x.IdFundingSource)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AcquisitionClassification)
                   .WithMany()
                   .HasForeignKey(x => x.IdAcquisitionClassification)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Program)
                   .WithMany()
                   .HasForeignKey(x => x.IdProgram)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Community)
                   .WithMany()
                   .HasForeignKey(x => x.IdCommunity)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Beneficiary)
                   .WithMany()
                   .HasForeignKey(x => x.IdBeneficiary)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PaymentPolicy)
       .WithMany(x => x.Requests)
       .HasForeignKey(x => x.IdPaymentPolicy)
       .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // AUDIT FIELDS
            // =====================================================

            builder.Property(x => x.CreatedBy)
                   .HasColumnName("creadoPor");

            builder.Property(x => x.CreatedAt)
                   .HasColumnName("fechaCreacion")
                   .HasColumnType("datetime2");

            builder.Property(x => x.UpdatedBy)
                   .HasColumnName("modificadoPor");

            builder.Property(x => x.UpdatedAt)
                   .HasColumnName("fechaModificacion")
                   .HasColumnType("datetime2");

            builder.Property(x => x.Active)
                   .HasColumnName("activo");
        }
    }
}
