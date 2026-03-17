using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion
{
    public record NotificationDto(
        int Id,
        string Title,
        string Message,
        int? RequestId,
        bool IsRead,
        DateTime CreatedAt
    );
}
