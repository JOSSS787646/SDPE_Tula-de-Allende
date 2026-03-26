using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IANotification;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // ============================================================
        // 🔥 ELIMINACIÓN MASIVA (UNA SOLA FILA)
        // ============================================================
        public async Task LogNotificationDeletionAsync(IEnumerable<Notification> notifications)
        {
            if (notifications == null || !notifications.Any())
                return;

            var userId = _currentUserService.UserId;
            var userEmail = _currentUserService.Email; // 🔥 AQUÍ
            var first = notifications.First();

            var history = new NotificationHistory
            {
                NotificationId = null,
                TargetUserId = first.UserId,

                Title = "Eliminación masiva",
                Message = $"Se eliminaron {notifications.Count()} notificaciones",

                RequestId = null,
                CreatedAt = DateTime.UtcNow,

                DeletedByUserId = userId,
                DeletedByUserEmail = userEmail, // 🔥 AHORA SÍ

                DeletedAt = DateTime.UtcNow,
                Action = "DELETED_ALL"
            };

            await _historyRepository.AddAsync(history);
        }

        // ============================================================
        // 🔥 ELIMINACIÓN INDIVIDUAL
        // ============================================================
        public async Task LogSingleNotificationDeletionAsync(Notification notification)
        {
            if (notification == null)
                return;

            var userId = _currentUserService.UserId;
            var userEmail = _currentUserService.Email;

            var history = new NotificationHistory
            {
                NotificationId = notification.IdNotification,
                TargetUserId = notification.UserId,

                Title = notification.Title,
                Message = notification.Message,

                RequestId = notification.RequestId,
                CreatedAt = notification.CreatedAt,

                DeletedByUserId = userId,
                DeletedByUserEmail = userEmail,

                DeletedAt = DateTime.UtcNow,
                Action = "DELETED"
            };

            await _historyRepository.AddAsync(history);
        }
    }
}