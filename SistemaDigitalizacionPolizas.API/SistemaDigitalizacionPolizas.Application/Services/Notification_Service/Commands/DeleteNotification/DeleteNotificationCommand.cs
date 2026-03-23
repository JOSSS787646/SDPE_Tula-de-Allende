using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.DeleteNotification
{

    public record DeleteNotificationCommand(int NotificationId) : IRequest<bool>;
}
