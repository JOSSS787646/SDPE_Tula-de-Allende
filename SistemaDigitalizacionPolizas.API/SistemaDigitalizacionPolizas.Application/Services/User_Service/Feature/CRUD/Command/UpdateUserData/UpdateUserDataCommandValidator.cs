using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Command.UpdateUserData
{
    public class UpdateUserDataCommandValidator
         : AbstractValidator<UpdateUserDataCommand>
    {
        public UpdateUserDataCommandValidator()
        {
            // 🔑 Id del usuario
            RuleFor(x => x.IdUser)
                .GreaterThan(0)
                .WithMessage("El identificador del usuario debe ser mayor que cero.");

            // 📧 Correo electrónico
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress()
                .WithMessage("El formato del correo electrónico no es válido.")
                .MaximumLength(100)
                .WithMessage("El correo electrónico no debe exceder los 100 caracteres.");

            // 🧑‍💼 Rol
            RuleFor(x => x.IdRole)
                .GreaterThan(0)
                .WithMessage("El identificador del rol debe ser mayor que cero.");

            // 🏢 Unidad administrativa
            RuleFor(x => x.IdAdministrativeUnit)
                .GreaterThan(0)
                .WithMessage("El identificador de la unidad administrativa debe ser mayor que cero.");
        }
    }
}
