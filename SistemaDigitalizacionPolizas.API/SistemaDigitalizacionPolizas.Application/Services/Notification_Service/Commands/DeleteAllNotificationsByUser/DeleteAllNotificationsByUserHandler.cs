using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.DeleteAllNotificationsByUser
{
    public class DeleteAllNotificationsByUserHandler
      : IRequestHandler<DeleteAllNotificationsByUserCommand>
    {
        private readonly INotificationRepository _repository;

        public DeleteAllNotificationsByUserHandler(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            DeleteAllNotificationsByUserCommand request,
            CancellationToken cancellationToken)
        {
            await _repository.DeleteAllByUserAsync(request.UserId);

            return Unit.Value;
        }
    }
}
