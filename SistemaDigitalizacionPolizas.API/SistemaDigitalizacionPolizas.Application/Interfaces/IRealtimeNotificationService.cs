using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Interfaces
{
    public interface IRealtimeNotificationService
    {
        Task SendAsync(Notification notification);
    }
}
