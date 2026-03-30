using SistemaDigitalizacionPolizas.Application.Interfaces;
using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.CreateNotification
{
    public class CreateNotificationHandler
        : IRequestHandler<CreateNotificationCommand, int>
    {
        private readonly INotificationRepository _repository;
        private readonly IRealtimeNotificationService _realtime;
        private readonly ICurrentUserService _currentUser;

        public CreateNotificationHandler(
            INotificationRepository repository,
            IRealtimeNotificationService realtime,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _realtime = realtime;
            _currentUser = currentUser;
        }

        public async Task<int> Handle(
            CreateNotificationCommand request,
            CancellationToken cancellationToken)
        {
            var notification = new Notification
            {
                UserId = request.UserId,
                Title = request.Title,
                Message = request.Message,
                RequestId = request.RequestId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy=_currentUser.UserId,
                IsRead = false,
                Active = true
            };

            // 💾 Guardar en BD
            await _repository.AddAsync(notification);

            // ⚡ Enviar en tiempo real (con toda la info)
            await _realtime.SendAsync(notification);

            // 🔥 regresar Id (útil si lo necesitas)
            return notification.IdNotification;
        }
    }
}
