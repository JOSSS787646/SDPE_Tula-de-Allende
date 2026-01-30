using FluentValidation;

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
