using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument
{
    public class ExpedientDocumentPreviewDto
    {
        public int IdDocument { get; set; }

        public string DocumentName { get; set; } = null!;
    }
}
