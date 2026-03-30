using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Pdf_Service.Commands.GenerateAcquisitionRequestFormPdf
{
    public class GenerateAcquisitionRequestFormPdfValidator
        : AbstractValidator<GenerateAcquisitionRequestFormPdfCommand>
    {
        public GenerateAcquisitionRequestFormPdfValidator()
        {
            RuleFor(x => x.RequestId)
                .GreaterThan(0)
                .WithMessage("El ID de la solicitud es obligatorio");
        }
    }
}
