using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands
{
    public class CreateRequestDocumentExceptionValidator
        : AbstractValidator<CreateRequestDocumentExceptionCommand>
    {
        public CreateRequestDocumentExceptionValidator()
        {
            RuleFor(x => x.IdRequest)
                .GreaterThan(0)
                .WithMessage("El Id de la solicitud es obligatorio.");

            RuleFor(x => x.IdDocumentType)
                .GreaterThan(0)
                .WithMessage("El Id del tipo de documento es obligatorio.");

            RuleFor(x => x.Justification)
                .NotEmpty()
                .WithMessage("La justificación es obligatoria.")
                .MaximumLength(500)
                .WithMessage("La justificación no puede exceder 500 caracteres.");

            RuleFor(x => x.DoesNotApply)
                .NotNull()
                .WithMessage("Debe indicar si el documento no aplica.");
        }
    }
}
