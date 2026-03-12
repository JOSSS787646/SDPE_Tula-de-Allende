using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest
{
    public class AcquisitionRequestDetailDto
    {
        public int IdRequest { get; set; }
        public string? RequestNumber { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? Justification { get; set; }
        public DateTime? AuthorizationDate { get; set; }
        public string? Observations { get; set; }
        public string? CFDI { get; set; }

        public string? PolicyNumber { get; set; }

        public SimpleCatalogDto? CurrentRequestStatus { get; set; }

        // 🔹 Relaciones (solo Id + Nombre)
        public SimpleCatalogDto? AdministrativeUnit { get; set; }
        public SimpleCatalogDto? Proyect { get; set; }
        public SimpleCatalogDto? AcquisitionType { get; set; }
        public SimpleCatalogDto? Supplier { get; set; }
        public SimpleCatalogDto? ApplicationStatus { get; set; }
        public SimpleCatalogDto? FundingSource { get; set; }
        public SimpleCatalogDto? AcquisitionClassification { get; set; }
        public SimpleCatalogDto? Program { get; set; }
        public SimpleCatalogDto? Community { get; set; }
        public SimpleCatalogDto? Beneficiary { get; set; }
    }

    // DTO genérico reutilizable para catálogos
    public class SimpleCatalogDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
