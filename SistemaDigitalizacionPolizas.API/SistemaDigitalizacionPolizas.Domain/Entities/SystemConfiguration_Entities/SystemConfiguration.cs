using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Entities.SystemConfiguration_Entities
{
    public class SystemConfiguration
    {
        public int IdConfiguration { get; set; }

        public bool EmailsEnabled { get; set; }

        public DateTime? NotificationStartDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public bool Active { get; set; }
    }
}
