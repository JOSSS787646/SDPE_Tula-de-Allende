using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest
{
    public class CreateAcquisitionRequestDto
    {
        // ===============================
        // Información General
        // ===============================

        public string? RequestNumber { get; set; }
        public DateTime? RequestDate { get; set; }

        public string? Justification { get; set; }
        public DateTime? AuthorizationDate { get; set; }

        public string? Observations { get; set; }

        public DateTime? CompleteMaximeDate { get; set; }

        public string? CFDI { get; set; }

        // ===============================
        // Claves Foráneas (Opcionales)
        // ===============================

        public int? IdAdministrativeUnit { get; set; }
        public int? IdProject { get; set; }
        public int? IdAcquisitionType { get; set; }
        public int? IdSupplier { get; set; }
        public int? IdApplicationStatus { get; set; }
        public int? IdFundingSource { get; set; }
        public int? IdAcquisitionClassification { get; set; }
        public int? IdProgram { get; set; }
        public int? IdCommunity { get; set; }
        public int? IdBeneficiary { get; set; }
        public int? IdPayementPolicy { get; set; }
    }
}
