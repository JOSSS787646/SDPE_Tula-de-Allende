using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion
{
    public class DocumentObservedNotificationDto
    {
        public string RequestNumber { get; set; } = null!;
        
        public string? ManagerEmail { get; set; }

        public string ManagerFullName { get; set; } = null!;

        public string? AdminEmail { get; set; }
    }
}
