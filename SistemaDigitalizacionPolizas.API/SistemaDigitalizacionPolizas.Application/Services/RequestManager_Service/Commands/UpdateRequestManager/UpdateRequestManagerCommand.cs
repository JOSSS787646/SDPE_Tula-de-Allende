using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.CreateRequestManager_Service.Commands.UpdateRequestManager
{
    public record UpdateRequestManagerCommand(
    int IdRequestManager,
    int? IdAdministrativeUnit,
    string FirstName,
    string LastName,
    string? SecondLastName,
    string? Email,
    string? Phone
) : IRequest<bool>;

}
