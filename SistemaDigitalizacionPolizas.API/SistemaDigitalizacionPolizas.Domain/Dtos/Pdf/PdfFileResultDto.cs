using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.Pdf
{
    public class PdfFileResultDto
    {
        public byte[] Content { get; set; } = default!;
        public string FileName { get; set; } = string.Empty;
    }
}
