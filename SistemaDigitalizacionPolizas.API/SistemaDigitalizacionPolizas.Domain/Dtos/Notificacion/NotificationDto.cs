using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion
{
    public class NotificationDto
    {
        public int IdNotification { get; set; }

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public bool IsRead { get; set; }
       
        public int? RequestId { get; set; }

    }
}
