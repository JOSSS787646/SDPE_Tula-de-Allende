using SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestNotification
{


    public interface IRequestNotificationRepository
    {
        Task<RequestNotificationDto?> GetRequestNotificationInfoAsync(int requestId);
        Task<DocumentApprovedNotificationDto?> GetDocumentApprovedInfoAsync(int requestId);
        Task<DocumentObservedNotificationDto?> GetDocumentObservedInfoAsync(int requestId);
    }
}
