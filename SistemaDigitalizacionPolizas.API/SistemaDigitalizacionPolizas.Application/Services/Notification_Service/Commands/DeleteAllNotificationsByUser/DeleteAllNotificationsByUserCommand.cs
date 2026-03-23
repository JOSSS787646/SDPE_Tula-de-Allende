using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.DeleteAllNotificationsByUser
{
    public record DeleteAllNotificationsByUserCommand(int UserId) : IRequest;
}
