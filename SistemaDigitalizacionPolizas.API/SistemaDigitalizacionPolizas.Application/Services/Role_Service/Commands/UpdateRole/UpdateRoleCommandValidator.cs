using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Role_Service.Commands.UpdateRole
{
    public class UpdateRoleCommandValidator
     : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(x => x.IdRol)
                .GreaterThan(0).WithMessage("El Id del rol es inválido.");

            RuleFor(x => x.RolName)
                .NotEmpty().WithMessage("El nombre del rol es obligatorio.")
                .MaximumLength(50);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(150);
        }
    }
}
