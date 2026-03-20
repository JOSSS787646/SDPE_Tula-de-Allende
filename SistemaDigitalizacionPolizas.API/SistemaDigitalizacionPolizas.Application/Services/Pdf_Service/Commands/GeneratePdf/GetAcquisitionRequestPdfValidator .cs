using FluentValidation;
using SistemaDigitalizacionPolizas.Domain.Dtos.Pdf;
using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Pdf_Service.Query.GetAcquisitionRequestPdf
{
    public class GetAcquisitionRequestPdfValidator
        : AbstractValidator<GetAcquisitionRequestPdfCommand>
    {
        public GetAcquisitionRequestPdfValidator()
        {
            RuleFor(x => x.RequestId)
                .GreaterThan(0)
                .WithMessage("RequestId must be greater than 0");
        }
    }
}
