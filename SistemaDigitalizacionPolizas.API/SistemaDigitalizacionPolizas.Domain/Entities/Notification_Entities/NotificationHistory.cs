using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities
{
    public class NotificationHistory
    {
        public int Id { get; set; }

        public int? NotificationId { get; set; } // original IdNotificacion
        public int? TargetUserId { get; set; } // IdUsuarioNotificacion
        public string? TargetUserName { get; set; }

        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;

        public int? RequestId { get; set; } // IdSolicitud
        public string? RequestNumber { get; set; }
        public string DeletedByUserEmail { get; set; }
        public DateTime CreatedAt { get; set; }

        public int DeletedByUserId { get; set; }
        public DateTime DeletedAt { get; set; }

        public string Action { get; set; } = null!;

        // 🔗 Navigation
        public virtual User DeletedByUser { get; set; } = null!;
    }
}
