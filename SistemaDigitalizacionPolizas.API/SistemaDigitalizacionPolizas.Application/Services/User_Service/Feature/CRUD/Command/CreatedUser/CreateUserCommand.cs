using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Command.CreatedUser
{
    public record CreateUserCommand(
       string Email,
       string Password,
       int IdAdministrativeUnit,
       int IdRole
   ) : IRequest<int>;

}
