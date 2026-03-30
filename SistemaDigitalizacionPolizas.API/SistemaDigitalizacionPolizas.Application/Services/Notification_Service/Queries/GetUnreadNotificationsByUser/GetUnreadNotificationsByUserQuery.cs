using SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion;
using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Queries.GetUnreadNotificationsByUser
{
    public record GetUnreadNotificationsByUserQuery(int UserId)
      : IRequest<List<NotificationDto>>;
}
