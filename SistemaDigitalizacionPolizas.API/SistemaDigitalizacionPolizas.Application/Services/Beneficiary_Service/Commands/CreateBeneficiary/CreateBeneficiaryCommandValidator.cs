using FluentValidation;

namespace SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Commands.CreateBeneficiary
{
    public class CreateBeneficiaryCommandValidator
        : AbstractValidator<CreateBeneficiaryCommand>
    {
        public CreateBeneficiaryCommandValidator()
        {
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
        }
    }
}
