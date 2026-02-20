using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Entities.ApplicationStatus
{
    public class ApplicationStatus
    {
        public int IdApplicationStatus { get; set; }
        public int Code { get; set; }
        public string Description { get; set; } 
        public int Order { get; set; }
        
        public bool Active { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
