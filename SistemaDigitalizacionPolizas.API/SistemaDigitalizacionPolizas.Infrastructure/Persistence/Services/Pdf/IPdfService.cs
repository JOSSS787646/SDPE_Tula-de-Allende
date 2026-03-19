using SistemaDigitalizacionPolizas.Domain.Dtos.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.Pdf
{
    public interface IPdfService
    {
        byte[] GenerateAcquisitionRequestPdf(AcquisitionRequestPdfDto dto);
    }
}
