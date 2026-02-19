using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.Beneficiary
{
    public class CreateBeneficiaryDto
    {
        public int IdBeneficiary { get; set; }

        public string FirstName { get; set; } = null!;
        public string PaternalLastName { get; set; } = null!;
        public string? MaternalLastName { get; set; }

        public string Street { get; set; } = null!;
        public string? ExternalNumber { get; set; }
        public string? InternalNumber { get; set; }
        public string? Neighborhood { get; set; }
        public int PostalCode { get; set; }

        public string? City { get; set; }
        public string Municipality { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Country { get; set; } = null!;

        public string Ine { get; set; } = null!;
        public string Curp { get; set; } = null!;

        public string? Phone { get; set; }
        public string? Email { get; set; }

        public bool Active { get; set; } = true;

        public int? IdCommunity { get; set; }

   
    }
}
