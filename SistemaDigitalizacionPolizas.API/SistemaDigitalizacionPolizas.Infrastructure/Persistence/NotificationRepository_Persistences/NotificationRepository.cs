using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.NotificationRepository_Persistences
{
    public class NotificationRepository: INotificationRepository
    {

        private readonly SdpeDbContext _context;

        public NotificationRepository(SdpeDbContext context)
        {
            _context = context;
        }


        public async Task<List<Notification>> GetByUserAsync(int userId)
        {
            return await _context.Notifications
                .Where(x => x.UserId == userId && x.Active)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(x => x.IdNotification == notificationId);

            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.Notifications
                .CountAsync(x => x.UserId == userId && !x.IsRead && x.Active);
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(x => x.UserId == userId && !x.IsRead)
                .ToListAsync();

            foreach (var n in notifications)
                n.IsRead = true;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int notificationId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(x => x.IdNotification == notificationId);

            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAllByUserAsync(int userId)
        {
            await _context.Notifications
                .Where(x => x.UserId == userId)
                .ExecuteDeleteAsync();
        }

    }
}
