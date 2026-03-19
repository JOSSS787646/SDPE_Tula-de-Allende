using System;
using System.Collections.Generic;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.Pdf
{
    public class AcquisitionRequestPdfDto
    {
        // ===============================
        // Identification
        // ===============================
        public string? Folio { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? AuthorizationDate { get; set; }
        public DateTime? MaxCompletionDate { get; set; }

        // ===============================
        // General Information
        // ===============================
        public string? AdministrativeUnit { get; set; }
        public string? Program { get; set; }
        public string? Project { get; set; }
        public string? FundingSource { get; set; }
        public string? AcquisitionType { get; set; }
        public string? Classification { get; set; }

        // ===============================
        // Location / Beneficiary
        // ===============================
        public string? Community { get; set; }
        public string? Beneficiary { get; set; }

        // ===============================
        // Supplier / Payment
        // ===============================
        public string? Supplier { get; set; }
        public string? SupplierRFC { get; set; }
        public string? PaymentPolicy { get; set; }
        public string? CFDI { get; set; }

        // ===============================
        // Content
        // ===============================
        public string? Justification { get; set; }
        public string? Observations { get; set; }

        // ===============================
        // Status
        // ===============================
        public string? Status { get; set; }

        // ===============================
        // Audit
        // ===============================
        public DateTime CreatedAt { get; set; }
        public string? CreatedByName { get; set; }

        // ===============================
        // Totals
        // ===============================
        public decimal TotalAmount { get; set; }

        // ===============================
        // Details
        // ===============================
        public List<AcquisitionRequestPdfItemDto> Items { get; set; } = new();
    }

    // =====================================
    // DETAIL DTO
    // =====================================
    public class AcquisitionRequestPdfItemDto
    {
        public decimal Quantity { get; set; }
        public string UnitMeasure { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }

        // Optional (Government classification)
        public string? CogKey { get; set; }
        public string? CogName { get; set; }
    }
}