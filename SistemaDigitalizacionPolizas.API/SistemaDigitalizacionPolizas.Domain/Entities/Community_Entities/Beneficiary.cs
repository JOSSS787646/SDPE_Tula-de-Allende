using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities
{
    public class Beneficiary
    {
        public int IdBeneficiary { get; set; }

        // ===============================
        // Datos personales
        // ===============================

        public string FirstName { get; set; } = null!;
        public string PaternalLastName { get; set; } = null!;
        public string? MaternalLastName { get; set; }

        // ===============================
        // Dirección
        // ===============================

        public string Street { get; set; } = null!;
        public string? ExternalNumber { get; set; }
        public string? InternalNumber { get; set; }
        public string? Neighborhood { get; set; }
        public int PostalCode { get; set; }

        public string? City { get; set; }
        public string Municipality { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Country { get; set; } = null!;

        // ===============================
        // Identificación
        // ===============================

        public string Ine { get; set; } = null!;
        public string Curp { get; set; } = null!;

        public string? Phone { get; set; }
        public string? Email { get; set; }

        // ===============================
        // Estado
        // ===============================

        public bool Active { get; set; } = true;

        // ===============================
        // Auditoría
        // ===============================

        public int CreatedBy { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

}
