using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Command.UpdateStatusUser
{
    public record UpdateUserStatusCommand(UpdateUserStatusDto Data)
    : IRequest<bool>;
}
