using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Entities.Supplier_Entities
{
    public class Supplier
    {
        public int IdSupplier { get; set; }

        public string Rfc { get; set; }
        public string BusinessName { get; set; } = null!;

        public string Street { get; set; } = null!;
        public string? ExternalNumber { get; set; }
        public string? InternalNumber { get; set; }
        public string? Neighborhood { get; set; }
        public int PostalCode { get; set; }

        public string? City { get; set; }
        public string Municipality { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Country { get; set; } = null!;

        public string? Phone { get; set; }
        public string? ContactName { get; set; }
        public string? ContactPhone { get; set; }
        public string? Email { get; set; }

        public bool Active { get; set; } = true;
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
