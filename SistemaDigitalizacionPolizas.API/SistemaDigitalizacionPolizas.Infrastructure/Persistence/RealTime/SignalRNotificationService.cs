using Microsoft.AspNetCore.SignalR;
using SistemaDigitalizacionPolizas.API.SignalR;
using SistemaDigitalizacionPolizas.Application.Interfaces;
using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.RealTime
{
    public class SignalRNotificationService : IRealtimeNotificationService
    {
        private readonly IHubContext<NotificationHub> _hub;

        public SignalRNotificationService(
            IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task SendAsync(Notification notification)
        {
            await _hub.Clients
                .User(notification.UserId.ToString())
                .SendAsync("ReceiveNotification", new
                {
                    id = notification.IdNotification,
                    title = notification.Title,
                    message = notification.Message,
                    requestId = notification.RequestId, 
                    createdAt = notification.CreatedAt
                });
        }
    }
}
