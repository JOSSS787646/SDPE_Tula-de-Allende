using SistemaDigitalizacionPolizas.Application.Services.Audit_Service;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.DeleteNotification
{
    public class DeleteNotificationHandler
       : IRequestHandler<DeleteNotificationCommand, bool>
    {
        private readonly INotificationRepository _repository;
        private readonly INotificationHistoryService _historyService;

        public DeleteNotificationHandler(
            INotificationRepository repository,
            INotificationHistoryService historyService)
        {
            _repository = repository;
            _historyService = historyService;
        }

        public async Task<bool> Handle(
            DeleteNotificationCommand request,
            CancellationToken cancellationToken)
        {
            // 🔥 1. Obtener la notificación antes de eliminar
            var notification = await _repository.GetByIdAsync(request.NotificationId);

            if (notification == null)
                return false;

            // 🔥 2. Registrar historial
            await _historyService.LogSingleNotificationDeletionAsync(notification);

            // 🔥 3. Eliminar
            return await _repository.DeleteAsync(request.NotificationId);
        }
    }
}
