using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Entities.PaymentPolicy_Entities
{
    public class PaymentPolicy
    {
        public int Id { get; set; }

        public string PolicyCode { get; set; } = null!;

        public string? Description { get; set; }

        public string FilePath { get; set; } = null!;

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation (inverse optional)
        public AcquisitionRequest? Request { get; set; }
    }
}
