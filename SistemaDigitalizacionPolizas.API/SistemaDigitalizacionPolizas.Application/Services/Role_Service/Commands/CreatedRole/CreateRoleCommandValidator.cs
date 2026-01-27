using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Commands.CreatedRole
{
    public class CreateRoleCommandValidator
        : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.RolName)
                .NotEmpty().WithMessage("El nombre del rol es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre del rol no puede exceder 50 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .MaximumLength(150).WithMessage("La descripción no puede exceder 150 caracteres.");

            RuleFor(x => x.Active)
                .NotNull().WithMessage("El estado del rol es obligatorio.");
        }
    }
}
