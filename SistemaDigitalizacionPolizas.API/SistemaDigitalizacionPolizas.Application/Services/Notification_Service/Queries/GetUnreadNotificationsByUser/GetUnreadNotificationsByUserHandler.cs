using MediatR;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Queries.GetUnreadNotificationsByUser;
using SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;

namespace SistemaDigitalizacionPolizas.Application.Features.Notifications.Queries.GetUnreadByUser
{
    public class GetUnreadNotificationsByUserHandler
        : IRequestHandler<GetUnreadNotificationsByUserQuery, List<NotificationDto>>
    {
        private readonly INotificationRepository _repository;

        public GetUnreadNotificationsByUserHandler(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<NotificationDto>> Handle(
            GetUnreadNotificationsByUserQuery request,
            CancellationToken cancellationToken)
        {
            var notifications = await _repository.GetUnreadByUserAsync(request.UserId);

            return notifications.Select(n => new NotificationDto
            {
                IdNotification = n.IdNotification,
                Message = n.Message,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead,
                RequestId = n.RequestId,
            }).ToList();
        }
    }
}