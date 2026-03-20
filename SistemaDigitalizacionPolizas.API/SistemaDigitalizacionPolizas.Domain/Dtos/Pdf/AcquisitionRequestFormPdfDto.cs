using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.Pdf
{
    // ─────────────────────────────────────────────────────────
    //  DOCUMENTO 1 — Formulario físico (réplica del impreso)
    // ─────────────────────────────────────────────────────────
    public class AcquisitionRequestFormPdfDto
    {
        // ── Encabezado ────────────────────────────────────────
        public string? Folio { get; set; }                      // Nº 08236

        // ── Sección SOLICITUD — Datos de la UA ───────────────
        public string? UnitKey { get; set; }                    // Clave de la Unidad Admva.
        public string? UnitName { get; set; }                   // Nombre de la Unidad Solicitante
        public string? FundingSource { get; set; }              // Fuente de Financiamiento
        public string? Program { get; set; }                    // PROG
        public string? CogKey { get; set; }                     // COG
        public string? Project { get; set; }                    // Proyecto
        public DateTime? RequestDate { get; set; }              // Fecha de Solicitud

        // ── Justificación ────────────────────────────────────
        public string? Justification { get; set; }

        // ── Tabla de materiales/servicios ────────────────────
        public List<AcquisitionFormItemDto> Items { get; set; } = new();

        // ── Firmas de solicitud ───────────────────────────────
        public string? ReceivedAndQuotedBy { get; set; }       // Recibido y Cotizado (nombre)
        public string? ApplicantName { get; set; }              // Nombre y Firma del Titular

        // ── Sección VALIDACIÓN ───────────────────────────────
        public string? AuthorizedBy { get; set; }               // Tesorero Municipal (nombre)
        public string? ReceivedSignedBy { get; set; }           // Sello y Firma de Recibido
        public string? QuotationResponsible { get; set; }       // Director de Adquisiciones

        // ── COMPROMETIDO ─────────────────────────────────────
        public List<AcquisitionFormItemDto> CommittedItems { get; set; } = new();
        public DateTime? OrderDate { get; set; }                // Fecha de Pedido

        // ── DEVENGADO ────────────────────────────────────────
        public List<AcquisitionFormItemDto> AccruedItems { get; set; } = new();
        public DateTime? ReceivedDate { get; set; }             // Fecha de Recibido

        // ── EJERCIDO / PAGADO ────────────────────────────────
        public string? Supplier { get; set; }                   // Proveedor
        public string? SupplierRFC { get; set; }                // R.F.C.
        public bool? AuthorizedForPayment { get; set; }         // Autorizado para Pago
        public string? PaymentMethod { get; set; }              // Forma de Pago general
        public decimal? CashAmount { get; set; }                // Contado
        public decimal? CreditAmount { get; set; }              // Crédito
        public decimal? ReimbursementAmount { get; set; }       // Reembolso
        public decimal? SupplierPaymentAmount { get; set; }     // Pago a Proveedor
    }

    // ─────────────────────────────────────────────────────────
    //  Item compartido (Solicitud, Comprometido, Devengado)
    // ─────────────────────────────────────────────────────────
    public class AcquisitionFormItemDto
    {
        public decimal Quantity { get; set; }
        public string UnitMeasure { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }

    // ─────────────────────────────────────────────────────────
    //  DOCUMENTO 2 — Resumen completo (el que ya tenías)
    //  AcquisitionRequestPdfDto  →  no se modifica, ya existe
    // ─────────────────────────────────────────────────────────
}

