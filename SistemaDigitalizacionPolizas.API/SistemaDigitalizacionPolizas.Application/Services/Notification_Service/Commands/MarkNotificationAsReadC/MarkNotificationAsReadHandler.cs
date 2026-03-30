using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.MarkNotificationAsReadC
{
    public class MarkNotificationAsReadHandler
        : IRequestHandler<MarkNotificationAsReadCommand>
    {
        private readonly INotificationRepository _repository;

        public MarkNotificationAsReadHandler(INotificationRepository repository)
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
