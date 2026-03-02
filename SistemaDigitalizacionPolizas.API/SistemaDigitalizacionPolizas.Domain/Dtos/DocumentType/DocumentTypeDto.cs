using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.DocumentType
{
    public class DocumentTypeDto
    {
            
        public int IdDocumentType { get; set; }

        public string DocumentName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool Active { get; set; } = true;
    }
}
