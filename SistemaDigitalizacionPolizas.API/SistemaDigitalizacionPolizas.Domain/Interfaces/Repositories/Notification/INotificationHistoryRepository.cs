using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IANotification
{
    public interface INotificationHistoryRepository
    {
        Task AddAsync(NotificationHistory entity);
        Task AddRangeAsync(IEnumerable<NotificationHistory> entities);
    }
}
