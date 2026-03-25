using SistemaDigitalizacionPolizas.Domain.Dtos.ApplicationStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest
{
    public class RequestDocumentChecklistDto
    {
        public int DocumentTypeId { get; set; }

        public string DocumentName { get; set; } = null!;

        public bool RequiredByRule { get; set; }

        public bool NoApplies { get; set; }

        public bool Uploaded { get; set; }

        // 🔥 Todo agrupado por archivo
        public List<DocumentFileDto> Files { get; set; } = new();
    }

    public class DocumentFileDto
    {
        public int FileId { get; set; }

        public string FileName { get; set; } = null!;

        public string FileUrl { get; set; } = null!;

        public string PreviewUrl { get; set; } = null!;

        // 🔹 Observación al cargar
        public string? Observation { get; set; }

        // 🔥 Observación en revisión
        public string? ObservationUpload { get; set; }

        // 🔥 Status simplificado
        public DocumentStatusSimpleDto? Status { get; set; }
    }

    public class DocumentStatusSimpleDto
    {
        public int Id { get; set; }

        public string Description { get; set; } = null!;
    }
}
