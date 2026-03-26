using MediatR;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;
using SistemaDigitalizacionPolizas.Application.Services.Audit_Service;
using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.DeleteAllNotificationsByUser
{
    public class DeleteAllNotificationsByUserHandler
        : IRequestHandler<DeleteAllNotificationsByUserCommand>
    {
        private readonly INotificationRepository _repository;
        private readonly INotificationHistoryService _historyService;

        public DeleteAllNotificationsByUserHandler(
            INotificationRepository repository,
            INotificationHistoryService historyService)
        {
            _repository = repository;
            _historyService = historyService;
        }

        public async Task<Unit> Handle(
     DeleteAllNotificationsByUserCommand request,
     CancellationToken cancellationToken)
        {
            // 🔥 1. Obtener todas las notificaciones
            var notifications = await _repository.GetAllByUserAsync(request.UserId);

            if (notifications != null && notifications.Any())
            {
                // 🔥 2. Guardar historial
                await _historyService.LogNotificationDeletionAsync(notifications);
            }

            // 🔥 3. Eliminar
            await _repository.DeleteAllByUserAsync(request.UserId);

            return Unit.Value;
        }
    }
}