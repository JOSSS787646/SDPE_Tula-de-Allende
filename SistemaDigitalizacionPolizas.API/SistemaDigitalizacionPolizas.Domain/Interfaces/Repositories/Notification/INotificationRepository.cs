using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);

        Task<List<Notification>> GetByUserAsync(int userId);

        Task<int> GetUnreadCountAsync(int userId);

        Task MarkAsReadAsync(int notificationId);

        Task MarkAllAsReadAsync(int userId);
    }
}
