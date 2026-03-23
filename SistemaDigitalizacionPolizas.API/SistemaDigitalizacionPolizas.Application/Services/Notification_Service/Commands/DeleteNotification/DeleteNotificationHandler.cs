using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.DeleteNotification
{
    public class DeleteNotificationHandler
    : IRequestHandler<DeleteNotificationCommand, bool>
    {
        private readonly INotificationRepository _repository;

        public DeleteNotificationHandler(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteNotificationCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(request.NotificationId);
        }
    }
}
