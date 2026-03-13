using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.MarkAllNotificationsAsRead
{
    public class MarkAllNotificationsAsReadCommandHandler
        : IRequestHandler<MarkAllNotificationsAsReadCommand, Unit>
    {
        private readonly INotificationRepository _repository;

        public MarkAllNotificationsAsReadCommandHandler(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            MarkAllNotificationsAsReadCommand request,
            CancellationToken cancellationToken)
        {
            await _repository.MarkAllAsReadAsync(request.UserId);

            return Unit.Value;
        }
    }
}
