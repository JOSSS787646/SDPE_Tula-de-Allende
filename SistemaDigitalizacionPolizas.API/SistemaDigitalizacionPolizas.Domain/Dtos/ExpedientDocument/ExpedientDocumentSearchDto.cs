using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument
{
    public class ExpedientDocumentSearchDto
    {
        public int Id { get; set; }

        public string FileName { get; set; } = null!;

        public string FilePath { get; set; } = null!;

        public DateTime? UploadDate { get; set; }

        public string? Observations { get; set; }

        public int? IdDocumentStatus { get; set; }

        public string DocumentStatusName { get; set; } = null!;
    }
}
