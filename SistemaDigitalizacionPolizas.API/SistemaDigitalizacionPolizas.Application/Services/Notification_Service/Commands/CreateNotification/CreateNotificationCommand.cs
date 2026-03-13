using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.CreateNotification_Service.Commands.CreateNotification
{
    using MediatR;

    public record CreateNotificationCommand(
        int UserId,
        string Title,
        string Message,
        int? RequestId
    ) : IRequest<int>;
}
