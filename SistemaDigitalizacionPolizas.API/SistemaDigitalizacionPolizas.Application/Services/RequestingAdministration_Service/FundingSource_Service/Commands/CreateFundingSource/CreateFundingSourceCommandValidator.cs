using FluentValidation;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.CreateCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.AddFundingSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Commands.CreateFundingSource
{
    internal class CreateFundingSourceCommandValidator
        : AbstractValidator<CreateFundingSourceCommand>
    {
        public CreateFundingSourceCommandValidator()
        {
            RuleFor(x => x.Code)
          .GreaterThan(0)
          .WithMessage("El código del COG debe ser mayor a 0.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("La descripción del COG es obligatoria.")
                .MaximumLength(150)
                .WithMessage("La descripción del COG no debe exceder 150 caracteres.");

            RuleFor(x => x.Active)
                .NotNull()
                .WithMessage("El estado Active es obligatorio.");
        }
    }
}
