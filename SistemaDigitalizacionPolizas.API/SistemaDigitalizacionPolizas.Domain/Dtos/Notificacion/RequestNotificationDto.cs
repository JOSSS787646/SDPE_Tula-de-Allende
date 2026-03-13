using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion
{
    public class RequestNotificationDto
    {
        public string RequestNumber { get; set; } = "";
        public string AdministrativeUnitName { get; set; } = "";
        public string Justification { get; set; } = "";

        public DateTime? RequestDate { get; set; } 
    }
}
