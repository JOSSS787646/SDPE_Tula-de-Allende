// ============================================================
//  AcquisitionChecklistPdfDto.cs
//  DTO para el Check List — Documento 3
// ============================================================
using System;
using System.Collections.Generic;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.Pdf
{
    /// <summary>
    /// DTO principal para el PDF del Check List de Adquisiciones.
    /// Los datos de cabecera vienen de AcquisitionRequestPolizaDto;
    /// los ítems vienen de GetChecklistByRequestAsync.
    /// </summary>
    public class AcquisitionChecklistPdfDto
    {
        // ── Cabecera (de AcquisitionRequestPolizaDto) ─────────
        public string? Folio { get; set; }

        /// <summary>
        /// Viene de AcquisitionRequestPolizaDto.AcquisitionClassification
        /// </summary>
        public string? ClassificationName { get; set; }

        public DateTime? RequestDate { get; set; }
        public string? Status { get; set; }

        // ── Datos de la solicitud (opcionales en PDF) ─────────
        public string? AdministrativeUnit { get; set; }
        public string? ApplicantName { get; set; }
        public string? Program { get; set; }
        public string? Project { get; set; }
        public string? ExceptionStatus { get; set; }

        // ── Ítems (de List<RequestDocumentChecklistDto>) ──────
        public List<ChecklistPdfItemDto> Checklist { get; set; } = new();

        // ── Firmas ────────────────────────────────────────────
        public string? AcquisitionsSignatoryName { get; set; }
        public string? AcquisitionsSignatoryTitle { get; set; }  // "Guadalupe Noguez Becerra / Adquisiciones"

        public string? TreasurySignatoryName { get; set; }
        public string? TreasurySignatoryTitle { get; set; }      // "Jaqueline Moreno Martínez / Tesorería"
    }

    /// <summary>
    /// Ítem del checklist — mapea RequestDocumentChecklistDto
    /// </summary>
    public class ChecklistPdfItemDto
    {
        public int Order { get; set; }                           // número de orden/posición
        public string DocumentName { get; set; } = string.Empty;
        public bool RequiredByRule { get; set; }
        public bool NoApplies { get; set; }
        public bool Uploaded { get; set; }
        public string? ExceptionStatus { get; set; }
        /// <summary>
        /// Estado consolidado (Code del DocumentStatus de mayor Order).
        /// Valores esperados: "APROBADO", "PENDIENTE", "EN_REVISION", "RECHAZADO"
        /// </summary>
        public string? GlobalStatus { get; set; }

        public List<string>? Observations { get; set; }         // Observations + ObservationsUpload
        public List<ChecklistFilePdfDto>? Files { get; set; }
    }

    public class ChecklistFilePdfDto
    {
        public int FileId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string? FileUrl { get; set; }
        public string? Status { get; set; }
    }
}