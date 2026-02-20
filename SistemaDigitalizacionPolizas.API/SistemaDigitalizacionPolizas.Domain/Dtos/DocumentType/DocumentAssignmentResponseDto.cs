using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.DocumentType
{
    public class DocumentAssignmentResponseDto
    {
        public int DocumentTypeId { get; set; }
        public string DocumentName { get; set; } = null!;
        public bool IsRequired { get; set; }
        public bool Active { get; set; }
    }
}
