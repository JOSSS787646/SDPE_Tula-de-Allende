using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest
{
    public class ApplicationDetailDto
    {
        public int IdDetail { get; set; }

        // 🔥 COG como catálogo (ID + Nombre)
        public SimpleCatalogDto? Cog { get; set; }

        public decimal Quantity { get; set; }

        public string UnitMeasure { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal UnitAmount { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
