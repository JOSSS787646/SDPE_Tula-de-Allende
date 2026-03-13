using SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Queries.GetNotificationsByUser
{
    public class GetNotificationsByUserQueryHandler
        : IRequestHandler<GetNotificationsByUserQuery, List<NotificationDto>>
    {
        private readonly INotificationRepository _repository;

        public GetNotificationsByUserQueryHandler(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<NotificationDto>> Handle(
            GetNotificationsByUserQuery request,
            CancellationToken cancellationToken)
        {
            var notifications = await _repository.GetByUserAsync(request.UserId);

            return notifications
                .Select(n => new NotificationDto(
                    n.IdNotification,
                    n.Title,
                    n.Message,
                    n.RequestId,
                    n.IsRead,
                    n.CreatedAt
                ))
                .ToList();
        }
    }
}
