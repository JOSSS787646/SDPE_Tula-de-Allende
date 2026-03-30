using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Queries.GetUnreadCount
{
    public class GetUnreadCountHandler
     : IRequestHandler<GetUnreadCountQuery, int>
    {
        private readonly INotificationRepository _repository;

        public GetUnreadCountHandler(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(
            GetUnreadCountQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetUnreadCountAsync(request.UserId);
        }
    }
}
