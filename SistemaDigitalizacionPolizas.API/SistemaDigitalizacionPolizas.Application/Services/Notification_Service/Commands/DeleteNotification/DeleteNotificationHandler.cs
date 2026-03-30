using MediatR;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;
using SistemaDigitalizacionPolizas.Application.Services.Audit_Service;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.DeleteNotification
{
    public class DeleteNotificationHandler
        : IRequestHandler<DeleteNotificationCommand, bool>
    {
        private readonly INotificationRepository _repository;
        private readonly INotificationHistoryService _historyService;
        private readonly IUnitOfWorkService _unitOfWork; // 🔥 FALTABA

        public DeleteNotificationHandler(
            INotificationRepository repository,
            INotificationHistoryService historyService,
            IUnitOfWorkService unitOfWork) // 🔥 INYECTAR
        {
            _repository = repository;
            _historyService = historyService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeleteNotificationCommand request,
            CancellationToken cancellationToken)
        {
            var notification = await _repository.GetByIdAsync(request.NotificationId);

            if (notification == null)
                return false;

            // 🔥 1. Guardar historial
            await _historyService.LogSingleNotificationDeletionAsync(notification);

            // 🔥 2. Eliminar
            await _repository.DeleteAsync(request.NotificationId);

            // 🔥 3. GUARDAR CAMBIOS (ESTO TE FALTABA)
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}