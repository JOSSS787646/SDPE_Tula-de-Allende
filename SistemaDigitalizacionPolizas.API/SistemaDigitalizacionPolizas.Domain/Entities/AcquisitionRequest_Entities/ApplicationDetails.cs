using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities
{
    public class ApplicationDetail
    {
        public int IdDetail { get; set; }

        public int ApplicationId { get; set; }

        public int CogId { get; set; }

        public decimal Quantity { get; set; }

        public string UnitMeasure { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal UnitAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool Active { get; set; }


        public COG Cog { get; set; }

        // 🔗 Navigation
        public AcquisitionRequest AcquisitionRequest { get; set; } = null!;
    }
}
