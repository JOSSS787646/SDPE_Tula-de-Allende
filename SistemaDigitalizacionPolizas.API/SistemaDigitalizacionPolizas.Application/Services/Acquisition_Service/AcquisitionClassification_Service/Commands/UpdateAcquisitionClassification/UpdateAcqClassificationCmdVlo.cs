using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionClassification_Service.Commands.UpdateAcquisitionClassification
{
    public class UpdateAcqClassificationCmdVlo
        :AbstractValidator<UpdateAcqClassificationCmd>
    {

        public UpdateAcqClassificationCmdVlo() {
            RuleFor(x => x.idUpdateAcquisitionClassification)
                   .GreaterThan(0)
                   .WithMessage("El id del tipo de adquisición es inválido.");

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
