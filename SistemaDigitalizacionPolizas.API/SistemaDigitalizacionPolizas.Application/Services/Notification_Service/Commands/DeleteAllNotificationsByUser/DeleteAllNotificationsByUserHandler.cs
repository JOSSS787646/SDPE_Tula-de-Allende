using MediatR;
using Microsoft.Extensions.Logging;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;
using SistemaDigitalizacionPolizas.Application.Services.Audit_Service;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.DeleteAllNotificationsByUser
{
    public class DeleteAllNotificationsByUserHandler
        : IRequestHandler<DeleteAllNotificationsByUserCommand>
    {
        private readonly INotificationRepository _repository;
        private readonly INotificationHistoryService _historyService;
        private readonly IUnitOfWorkService _unitOfWork;
     

        public DeleteAllNotificationsByUserHandler(
            INotificationRepository repository,
            INotificationHistoryService historyService,
            IUnitOfWorkService unitOfWork
           )
        {
            _repository = repository;
            _historyService = historyService;
            _unitOfWork = unitOfWork;
   
        }

        public async Task<Unit> Handle(
            DeleteAllNotificationsByUserCommand request,
            CancellationToken cancellationToken)
        {
         

            var notifications = await _repository.GetAllByUserAsync(request.UserId);

     

            if (notifications != null && notifications.Any())
            {
                await _historyService.LogNotificationDeletionAsync(notifications);
            }

            await _repository.DeleteAllByUserAsync(request.UserId);

             await _unitOfWork.SaveChangesAsync();


            return Unit.Value;
        }
    }
}