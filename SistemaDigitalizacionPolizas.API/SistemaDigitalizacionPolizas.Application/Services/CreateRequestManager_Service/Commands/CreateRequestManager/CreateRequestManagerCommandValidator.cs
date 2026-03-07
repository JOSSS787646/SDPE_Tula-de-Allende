using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.CreateRequestManager_Service.Commands.CreateRequestManager
{
    public class CreateRequestManagerCommandValidator
       : AbstractValidator<CreateRequestManagerCommand>
    {
        public CreateRequestManagerCommandValidator()
        {
            RuleFor(x => x.IdRequest)
                .GreaterThan(0)
                .WithMessage("The request id is required.");
            RuleFor(x => x.IdAdministrativeUnit)
              .GreaterThan(0)
              .WithMessage("The request id is required.");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required.")
                .MaximumLength(65);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.")
                .MaximumLength(65);

            RuleFor(x => x.SecondLastName)
                .MaximumLength(65);

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Phone)
                .Matches(@"^[0-9]{7,15}$")
                .WithMessage("Phone must contain only numbers.")
                .When(x => !string.IsNullOrWhiteSpace(x.Phone));
        }
    }
}
