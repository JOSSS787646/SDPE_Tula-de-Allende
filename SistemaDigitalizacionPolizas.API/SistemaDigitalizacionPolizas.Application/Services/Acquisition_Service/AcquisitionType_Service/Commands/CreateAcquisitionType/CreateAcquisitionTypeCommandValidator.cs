using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Commands.CreateAcquisitionType
{
    public class CreateAcquisitionTypeCommandValidator
    : AbstractValidator<CreateAcquisitionTypeCommand>
    {
        public CreateAcquisitionTypeCommandValidator()
        {
            RuleFor(x => x.Code)
                .GreaterThan(0)
                .WithMessage("La clave del COG debe ser mayor a 0.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("La descripción es obligatoria.")
                .MaximumLength(250)
                .WithMessage("La descripción no puede exceder 250 caracteres.");
        }
    }

}
