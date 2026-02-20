using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.DocumentType
{
    public class CreateExpedientDocumentDto
    {
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
        // Soft Delete
        // ================================
        public bool? Active { get; set; }
    }
}
