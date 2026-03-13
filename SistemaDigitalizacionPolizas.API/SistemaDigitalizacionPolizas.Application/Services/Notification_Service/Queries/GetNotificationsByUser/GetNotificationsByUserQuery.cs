using SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Queries.GetNotificationsByUser
{
    public record GetNotificationsByUserQuery(int UserId)
        : IRequest<List<NotificationDto>>;
}
