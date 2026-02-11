using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Supplier_Service.Commands.CreateSupplier
{

    public class CreateSupplierCommandValidator
      : AbstractValidator<CreateSupplierCommand>
    {
        public CreateSupplierCommandValidator()
        {
            RuleFor(x => x.Supplier)
                .NotNull().WithMessage("El proveedor es obligatorio.");

            When(x => x.Supplier != null, () =>
            {
                RuleFor(x => x.Supplier.Rfc)
                    .NotEmpty().WithMessage("El RFC es obligatorio.")
                    .MaximumLength(13).WithMessage("El RFC no debe exceder 13 caracteres.");

                RuleFor(x => x.Supplier.BusinessName)
                    .NotEmpty().WithMessage("La razón social es obligatoria.")
                    .MaximumLength(60);

                RuleFor(x => x.Supplier.Street)
                    .NotEmpty().WithMessage("La calle es obligatoria.")
                    .MaximumLength(150);

                RuleFor(x => x.Supplier.PostalCode)
                    .GreaterThan(0).WithMessage("El código postal debe ser mayor a 0.");

                RuleFor(x => x.Supplier.Municipality)
                    .NotEmpty().WithMessage("El municipio es obligatorio.")
                    .MaximumLength(45);

                RuleFor(x => x.Supplier.State)
                    .NotEmpty().WithMessage("El estado es obligatorio.")
                    .MaximumLength(45);

                RuleFor(x => x.Supplier.Country)
                    .NotEmpty().WithMessage("El país es obligatorio.")
                    .MaximumLength(45);

                RuleFor(x => x.Supplier.Phone)
                    .NotEmpty().WithMessage("El teléfono es obligatorio.")
                    .MaximumLength(20).WithMessage("El teléfono no debe exceder 20 caracteres.")
                    .Matches(@"^[0-9+()\-\s]+$")
                    .WithMessage("El teléfono solo puede contener números y símbolos básicos.");

                RuleFor(x => x.Supplier.Email)
                    .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Supplier.Email))
                    .WithMessage("El correo no tiene un formato válido.");
            });
        }
    }
}
        
