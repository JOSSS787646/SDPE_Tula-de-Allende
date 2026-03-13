using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaDigitalizacionPolizas.Application.Interfaces;

namespace SistemaDigitalizacionPolizas.Application.Services.CreateNotification_Service.Commands.CreateNotification
{
    public class CreateNotificationCommandHandler
    : IRequestHandler<CreateNotificationCommand, int>
    {
        private readonly INotificationRepository _repository;
        private readonly IRealtimeNotificationService _realtime;

        public CreateNotificationCommandHandler(
            INotificationRepository repository,
            IRealtimeNotificationService realtime)
        {
            _repository = repository;
            _realtime = realtime;
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
                IsRead = false,
                Active = true
            };

            await _repository.AddAsync(notification);

            await _realtime.SendAsync(
                request.UserId,
                request.Title,
                request.Message
            );

            return notification.IdNotification;
        }
    }
}
