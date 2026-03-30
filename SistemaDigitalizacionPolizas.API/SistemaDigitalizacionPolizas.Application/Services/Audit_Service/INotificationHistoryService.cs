using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Audit_Service
{
    public interface INotificationHistoryService
    {
        Task LogNotificationDeletionAsync(IEnumerable<Notification> notifications);

        Task LogSingleNotificationDeletionAsync(Notification notification);
    }
}
