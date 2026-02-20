using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.DocumentType
{
    public class DocumentAssignmentDto
    {
        public int DocumentTypeId { get; set; }
        public bool IsRequired { get; set; }
        public bool Active { get; set; } = true;
    }
}
