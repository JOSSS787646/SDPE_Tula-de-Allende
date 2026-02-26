using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.CreateMassiveExpedientDocument
{
    public class CreateMassiveExpedientDocumentCommandValidator
        : AbstractValidator<CreateMassiveExpedientDocumentCommand>
    {
        public CreateMassiveExpedientDocumentCommandValidator()
        {
            RuleFor(x => x.RequestId)
                .GreaterThan(0)
                .WithMessage("El RequestId es obligatorio.");

            RuleFor(x => x.Documents)
                .NotNull()
                .WithMessage("Debe enviar archivos.")
                .Must(x => x.Count > 0)
                .WithMessage("Debe enviar al menos un archivo.")
                .Must(x => x.Count <= 100)
                .WithMessage("Máximo 100 archivos permitidos por carga.");

            RuleForEach(x => x.Documents).ChildRules(doc =>
            {
                doc.RuleFor(d => d.File)
                    .NotNull()
                    .WithMessage("Uno de los archivos es inválido.");

                doc.RuleFor(d => d.File.Length)
                    .GreaterThan(0)
                    .WithMessage("Uno de los archivos está vacío.")
                    .LessThanOrEqualTo(20 * 1024 * 1024)
                    .WithMessage("Uno de los archivos supera el tamaño permitido (20MB).");
            });
        }
    }
}
