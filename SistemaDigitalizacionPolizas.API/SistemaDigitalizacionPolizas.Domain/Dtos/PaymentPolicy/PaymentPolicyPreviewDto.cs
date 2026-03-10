using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.PaymentPolicy
{
    public class PaymentPolicyPreviewDto
    {
        public int IdPaymentPolicy { get; set; }

        public string PolicyCode { get; set; } = null!;

        public string? Description { get; set; }

        public string PreviewUrl { get; set; } = null!;

    }
}
