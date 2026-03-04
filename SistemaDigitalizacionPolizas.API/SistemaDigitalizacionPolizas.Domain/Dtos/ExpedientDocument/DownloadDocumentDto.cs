using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument
{
    public record DownloadDocumentDto(
        string FileName,
        string ContentType,
        Stream FileStream
    );
}
