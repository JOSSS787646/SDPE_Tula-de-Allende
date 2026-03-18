using SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Queries.GetAllNotificationsByUser
{
    public record GetAllNotificationsByUserQuery(int UserId)
       : IRequest<List<NotificationDto>>;
}
