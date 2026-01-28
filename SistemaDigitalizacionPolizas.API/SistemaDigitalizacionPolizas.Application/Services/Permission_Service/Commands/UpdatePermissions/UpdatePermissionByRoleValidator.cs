using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Permission_Service.Commands.UpdatePermissions
{
    public class UpdatePermissionByRoleValidator
    : AbstractValidator<UpdatePermissionByRoleCommand>
    {
        public UpdatePermissionByRoleValidator()
        {
            RuleFor(x => x.IdRol)
                .GreaterThan(0);

            RuleFor(x => x.Permisos)
                .NotEmpty()
                .WithMessage("Debe seleccionar al menos un permiso.");
        }
    }
}
