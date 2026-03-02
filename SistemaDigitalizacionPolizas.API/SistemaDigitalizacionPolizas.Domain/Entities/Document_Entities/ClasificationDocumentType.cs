using SistemaDigitalizacionPolizas.Domain.Entities.Acquisition_Entities;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities
{
    public class ClasificationDocumentType
    {
        public int Id { get; set; }

        // 🔹 Foreign Keys
        public int ClassificationAcquisitionId { get; set; }
        public int? DocumentTypeId { get; set; }

        // 🔹 Business Rules
        public bool IsRequired { get; set; }
        // 🔹 Navigation Properties (opcional)
        public bool Active { get; set; } = true;
        public AcquisitionClassification ClassificationAcquisition { get; set; } = null!;
        public DocumentType DocumentType { get; set; } = null!;
    }
}
