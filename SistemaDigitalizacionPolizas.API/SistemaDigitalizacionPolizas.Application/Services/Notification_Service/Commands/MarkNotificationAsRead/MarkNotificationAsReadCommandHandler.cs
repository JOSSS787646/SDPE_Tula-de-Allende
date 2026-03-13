using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.MarkNotificationAsRead
{
    public class MarkNotificationAsReadCommandHandler
        : IRequestHandler<MarkNotificationAsReadCommand, Unit>
    {
        private readonly INotificationRepository _repository;

        public MarkNotificationAsReadCommandHandler(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            MarkNotificationAsReadCommand request,
            CancellationToken cancellationToken)
        {
            await _repository.MarkAsReadAsync(request.NotificationId);

            return Unit.Value;
        }
    }
}
