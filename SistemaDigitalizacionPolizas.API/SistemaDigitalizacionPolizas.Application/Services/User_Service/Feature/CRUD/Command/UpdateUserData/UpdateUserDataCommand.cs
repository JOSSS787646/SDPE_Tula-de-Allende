using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Command.UpdateUserData
{
    public record UpdateUserDataCommand
    (int IdUser,
    string Email,
    int IdRole,
    int IdAdministrativeUnit)
        : IRequest<bool>;

}
