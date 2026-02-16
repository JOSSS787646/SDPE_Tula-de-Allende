using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Commands.UpdateBeneficiary
{
    public class UpdateBeneficiaryCommandValidator
       : AbstractValidator<UpdateBeneficiaryCommand>
    {
        public UpdateBeneficiaryCommandValidator()
        {
            RuleFor(x => x.IdBeneficiary)
                .GreaterThan(0).WithMessage("El Id del beneficiario es inválido.");

            RuleFor(x => x.Beneficiary).NotNull();

            RuleFor(x => x.Beneficiary.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(65);

            RuleFor(x => x.Beneficiary.PaternalLastName)
                .NotEmpty().WithMessage("El apellido paterno es obligatorio.")
                .MaximumLength(65);

            RuleFor(x => x.Beneficiary.Street)
                .NotEmpty().WithMessage("La calle es obligatoria.")
                .MaximumLength(150);

            RuleFor(x => x.Beneficiary.PostalCode)
                .GreaterThan(0).WithMessage("El código postal es inválido.");

            RuleFor(x => x.Beneficiary.Municipality)
                .NotEmpty().WithMessage("El municipio es obligatorio.")
                .MaximumLength(45);

            RuleFor(x => x.Beneficiary.State)
                .NotEmpty().WithMessage("El estado es obligatorio.")
                .MaximumLength(45);

            RuleFor(x => x.Beneficiary.Country)
                .NotEmpty().WithMessage("El país es obligatorio.")
                .MaximumLength(45);

            RuleFor(x => x.Beneficiary.Curp)
                .NotEmpty().WithMessage("La CURP es obligatoria.")
                .Length(18).WithMessage("La CURP debe tener 18 caracteres.");

            RuleFor(x => x.Beneficiary.Ine)
                .NotEmpty().WithMessage("El INE es obligatorio.")
                .Length(13).WithMessage("El INE debe tener 13 caracteres.");

            RuleFor(x => x.Beneficiary.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Beneficiary.Email))
                .WithMessage("El correo no tiene un formato válido.");

            RuleFor(x => x.Beneficiary.IdBeneficiary)
    .Must((command, bodyId) => bodyId == 0 || bodyId == command.IdBeneficiary)
    .WithMessage("El id del cuerpo no puede ser diferente al de la ruta.");

        }
    }
}
