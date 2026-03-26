using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IANotification;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.NotificationRepository_Persistences
{
    public class NotificationHistoryRepository : INotificationHistoryRepository
    {
        private readonly SdpeDbContext _context;

        public NotificationHistoryRepository(SdpeDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(NotificationHistory entity)
        {
            await _context.NotificationHistories.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<NotificationHistory> entities)
        {
            await _context.NotificationHistories.AddRangeAsync(entities);
        }
    }
}
