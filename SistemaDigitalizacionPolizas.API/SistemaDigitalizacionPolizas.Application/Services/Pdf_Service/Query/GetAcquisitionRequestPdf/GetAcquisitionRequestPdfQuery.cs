using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Pdf_Service.Query.GetAcquisitionRequestPdf
{
    public record GetAcquisitionRequestPdfQuery(int RequestId) : IRequest<byte[]>;
}
