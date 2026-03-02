using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences
{
    public class DocumentType
    {
        public int IdDocumentType { get; set; }

        // 🔹 Business Data
        public string DocumentName { get; set; } = null!;
        public string Description { get; set; } = null!;
 

        // 🔹 Audit Fields
        public int CreatedBy { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // 🔹 Soft Delete
        public bool Active { get; set; } = true;

        public ICollection<ClasificationDocumentType> Classifications { get; set; }
    = new List<ClasificationDocumentType>();


        // 🔹 NUEVA navegación
        public ICollection<ExpedientDocument> ExpedientDocuments { get; set; }
            = new List<ExpedientDocument>();


        public ICollection<RequestDocumentException> RequestDocumentExceptions { get; set; }
    = new List<RequestDocumentException>();

    }
}
