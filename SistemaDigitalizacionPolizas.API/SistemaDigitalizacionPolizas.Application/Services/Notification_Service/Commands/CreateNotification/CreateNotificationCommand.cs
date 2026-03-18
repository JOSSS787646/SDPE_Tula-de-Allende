using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.CreateNotification
{
    public record CreateNotificationCommand(
      int UserId,
      string Title,
      string Message,
      int? RequestId
  ) : IRequest<int>;
}
