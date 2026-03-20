using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Pdf_Service.Commands.GenerateAcquisitionRequestFormPdf
{
    public record GenerateAcquisitionRequestFormPdfCommand(int RequestId) : IRequest<byte[]>;
}
