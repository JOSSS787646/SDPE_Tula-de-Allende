using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IANotification;

using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Audit_Service
{
    public class NotificationHistoryService : INotificationHistoryService
    {
        private readonly INotificationHistoryRepository _historyRepository;
        private readonly ICurrentUserService _currentUserService;

        public NotificationHistoryService(
            INotificationHistoryRepository historyRepository,
            ICurrentUserService currentUserService)
        {
            _historyRepository = historyRepository;
            _currentUserService = currentUserService;
        }

        // 🔥 BORRADO MASIVO
        public async Task LogNotificationDeletionAsync(IEnumerable<Notification> notifications)
        {
            if (notifications == null || !notifications.Any())
                return;

            var userId = _currentUserService.UserId;

            var histories = notifications.Select(n => new NotificationHistory
            {
                NotificationId = n.IdNotification,
                TargetUserId = n.UserId,

                Title = n.Title,
                Message = n.Message,

                RequestId = n.RequestId,
                CreatedAt = n.CreatedAt,

                DeletedByUserId = userId,
                DeletedAt = DateTime.UtcNow,

                Action = "DELETED"
            }).ToList();

            await _historyRepository.AddRangeAsync(histories);
        }

        // 🔥 BORRADO INDIVIDUAL
        public async Task LogSingleNotificationDeletionAsync(Notification notification)
        {
            if (notification == null)
                return;

            var userId = _currentUserService.UserId;

            var history = new NotificationHistory
            {
                NotificationId = notification.IdNotification, // ✅ CORREGIDO
                TargetUserId = notification.UserId,

                Title = notification.Title,
                Message = notification.Message,

                RequestId = notification.RequestId,
                CreatedAt = notification.CreatedAt,

                DeletedByUserId = userId,
                DeletedAt = DateTime.UtcNow,

                Action = "DELETED"
            };

            await _historyRepository.AddAsync(history);
        }
    }
}
