using SistemaDigitalizacionPolizas.Domain.Entities.Acquisition_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities
{
    public class AcquisitionClassificationType
    {
        public int IdAcquisitionClassificationType { get; set; }

        // 🔹 Foreign Keys
        public int? IdAcquisitionType { get; set; }
        public int? IdClassificationAcquisition { get; set; }

        // 🔹 Auditoría
        public int CreatedBy { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool Active { get; set; } = true;

        // 🔹 Navegación
        public AcquisitionType? AcquisitionType { get; set; }
        public AcquisitionClassification? AcquisitionClassification { get; set; }
    }

}
