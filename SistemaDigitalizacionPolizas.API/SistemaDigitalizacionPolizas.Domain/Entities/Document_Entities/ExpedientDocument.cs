using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities
{
    public class ExpedientDocument
    {
        // ================================
        // Primary Key
        // ================================
        public int Id { get; set; }

        // ================================
        // Foreign Keys
        // ================================
        public int? RequestId { get; set; }
        public int? DocumentTypeId { get; set; }

        // ================================
        // File Information
        // ================================
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime? UploadDate { get; set; }

        // ================================
        // Business Status
        // ================================
        public string? DocumentStatus { get; set; }
        public string? Observations { get; set; }

        // ================================
        // Audit Fields
        // ================================
        public int? UploadedBy { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // ================================
        // Soft Delete
        // ================================
        public bool? Active { get; set; }


        // ================================
        // Navigation
        // ================================
        public DocumentType? DocumentType { get; set; }

        // ⚠ Comentado hasta crear entidad Solicitud en dominio
        // public Solicitud? Request { get; set; }
    }
}
