using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.Areas_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Entities.RequestManager_Entities
{
    public class RequestManager
    {
        public int IdRequestManager { get; set; }

        public int IdRequest { get; set; }

        public int? IdAdministrativeUnit { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? SecondLastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        // Navigation
        public AcquisitionRequest Request { get; set; } = null!;

        public AdministrativeUnit? AdministrativeUnit { get; set; }
    }
}
