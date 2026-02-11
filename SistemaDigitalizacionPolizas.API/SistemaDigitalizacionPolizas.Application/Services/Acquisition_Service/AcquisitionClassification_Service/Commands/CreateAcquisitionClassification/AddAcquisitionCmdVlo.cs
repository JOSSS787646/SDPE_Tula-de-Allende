using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionClassification_Service.Commands.CreateAcquisitionClassification
{
    public class AddAcquisitionClassificationCmdValidator
      : AbstractValidator<AddAcquisitionClassificationCmd>
    {
        public AddAcquisitionClassificationCmdValidator()
        {
            RuleFor(x => x.Code)
                .GreaterThan(0)
                .WithMessage("El código es obligatorio y debe ser mayor a 0.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("La descripción es obligatoria.")
                .MaximumLength(250)
                .WithMessage("La descripción no debe exceder 250 caracteres.");
        }
    }
}
