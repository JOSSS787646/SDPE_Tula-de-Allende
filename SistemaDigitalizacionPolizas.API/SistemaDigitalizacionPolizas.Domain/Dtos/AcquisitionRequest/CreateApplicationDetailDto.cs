using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest
{
    public class CreateApplicationDetailDto
    {
        public int IdCog { get; set; }

        public int Quantity { get; set; }

        public string UnitMeasure { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal UnitAmount { get; set; }
    }
}
