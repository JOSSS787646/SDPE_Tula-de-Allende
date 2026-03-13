using Microsoft.AspNetCore.SignalR;
using SistemaDigitalizacionPolizas.API.SignalR;
using SistemaDigitalizacionPolizas.Application.Interfaces;
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

        public async Task SendAsync(int userId, string title, string message)
        {
            await _hub.Clients
                .User(userId.ToString())
                .SendAsync("ReceiveNotification", new
                {
                    title,
                    message
                });
        }
    }
}
