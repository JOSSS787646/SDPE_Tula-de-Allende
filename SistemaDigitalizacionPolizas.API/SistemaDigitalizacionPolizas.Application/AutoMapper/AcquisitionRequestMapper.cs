using SistemaDigitalizacionPolizas.Domain.Dtos.Pdf;
using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.AutoMapper
{
    public static class AcquisitionRequestMapper
    {
        public static AcquisitionRequestPdfDto ToPdfDto(AcquisitionRequest entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new AcquisitionRequestPdfDto
            {
                // ===============================
                // Identification
                // ===============================
                Folio = entity.RequestNumber,
                RequestDate = entity.RequestDate,
                AuthorizationDate = entity.AuthorizationDate,
                MaxCompletionDate = entity.CompleteMaximeDate,

                // ===============================
                // General Information
                // ===============================
                AdministrativeUnit = entity.AdministrativeUnit?.Description,
                Program = entity.Program?.Description,
                Project = entity.Project?.Description,
                FundingSource = entity.FundingSource?.Description,
                AcquisitionType = entity.AcquisitionType?.Description,
                Classification = entity.AcquisitionClassification?.Description,

                // ===============================
                // Location / Beneficiary
                // ===============================
                Community = entity.Community?.Description,
                Beneficiary = entity.Beneficiary?.FirstName,

                // ===============================
                // Supplier / Payment
                // ===============================
                Supplier = entity.Supplier?.ContactName,
                SupplierRFC = entity.Supplier?.Rfc,
                PaymentPolicy = entity.PaymentPolicy?.PolicyCode,
                CFDI = entity.CFDI,

                // ===============================
                // Content
                // ===============================
                Justification = entity.Justification,
                Observations = entity.Observations,

                // ===============================
                // Status
                // ===============================
                Status = entity.ApplicationStatus?.Description,

                // ===============================
                // Audit
                // ===============================
                CreatedAt = entity.CreatedAt,
                CreatedByName = entity.CreatedBy.ToString(), // 🔥 luego puedes mapear a usuario real

                // ===============================
                // Details
                // ===============================
                Items = entity.Details.Select(d => new AcquisitionRequestPdfItemDto
                {
                    Quantity = d.Quantity,
                    UnitMeasure = d.UnitMeasure,
                    Description = d.Description,
                    UnitPrice = d.UnitAmount,
                    Total = d.TotalAmount,

                    CogKey = d.Cog?.Description,
                    CogName = d.Cog?.Description
                }).ToList(),

                // ===============================
                // Totals
                // ===============================
                TotalAmount = entity.Details.Sum(d => d.TotalAmount)
            };
        }
    }
}
