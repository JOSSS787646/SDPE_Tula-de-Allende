using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.CreatedProyect
{
    public class CreatedProyectCommandValidator
      : AbstractValidator<CreatedProyectCommand>
    {
        public CreatedProyectCommandValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("El código es obligatorio.")
                .GreaterThan(0).WithMessage("El código debe ser mayor a cero.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .MaximumLength(250)
                .WithMessage("La descripción no puede exceder 250 caracteres.");
        }
    }
}
