using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.RequestManager
{
    public class CreateRequestManagerDto
    {
        public int IdRequest { get; set; }

        public int? IdAdministrativeUnit { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? SecondLastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}
