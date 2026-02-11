using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Supplier_Service.Commands.UpdateSupplier
{
    public class UpdateSupplierCommandValidator
        : AbstractValidator<UpdateSupplierCommand>
    {
        public UpdateSupplierCommandValidator()
        {
            RuleFor(x => x.Supplier).NotNull();

            RuleFor(x => x.Supplier.Rfc)
                .NotEmpty().Length(12, 13);

            RuleFor(x => x.Supplier.BusinessName)
                .NotEmpty().MaximumLength(60);

            RuleFor(x => x.Supplier.PostalCode)
                .GreaterThan(0);

            RuleFor(x => x.Supplier.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Supplier.Email));
        }
    }
}