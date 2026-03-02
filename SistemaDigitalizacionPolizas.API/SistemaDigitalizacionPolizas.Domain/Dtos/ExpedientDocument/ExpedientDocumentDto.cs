using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument
{
    public class ExpedientDocumentDto
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime? UploadDate { get; set; }

        public bool? Active { get; set; }

        // Relaciones “aplanadas” para la API
        public int? DocumentTypeId { get; set; }
        public string? DocumentTypeName { get; set; }

        public int? DocumentStatusId { get; set; }
        public string? DocumentStatusDescription { get; set; }
    }
}
