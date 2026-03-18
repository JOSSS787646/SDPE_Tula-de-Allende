using SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Queries.GetAllNotificationsByUser
{
    public class GetAllNotificationsByUserHandler
          : IRequestHandler<GetAllNotificationsByUserQuery, List<NotificationDto>>
    {
        private readonly INotificationRepository _repository;

        public GetAllNotificationsByUserHandler(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<NotificationDto>> Handle(
            GetAllNotificationsByUserQuery request,
            CancellationToken cancellationToken)
        {
            var notifications = await _repository.GetAllByUserAsync(request.UserId);

            return notifications.Select(n => new NotificationDto
            {
                IdNotification = n.IdNotification,
                Message = n.Message,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead,
                RequestId = n.RequestId 
            }).ToList();
        }
    }
}
