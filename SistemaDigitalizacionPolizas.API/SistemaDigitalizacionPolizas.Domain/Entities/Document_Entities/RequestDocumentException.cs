using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities
{
    public class RequestDocumentException
    {
        // 🔹 Primary Key
        public int IdRequestDocumentException { get; set; }

        // 🔹 Foreign Keys
        public int IdRequest { get; set; }
        public int IdDocumentType { get; set; }

        // 🔹 Business Data
        public bool DoesNotApply { get; set; }
        public string Justification { get; set; } = null!;

        // 🔹 Audit Fields
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        // 🔹 Soft Delete
        public bool Active { get; set; } = true;

        // 🔹 Navigation Properties (opcional, si ya tienes estas entidades)
        public AcquisitionRequest Request { get; set; } = null!;
        public DocumentType DocumentType { get; set; } = null!;
    }
}
