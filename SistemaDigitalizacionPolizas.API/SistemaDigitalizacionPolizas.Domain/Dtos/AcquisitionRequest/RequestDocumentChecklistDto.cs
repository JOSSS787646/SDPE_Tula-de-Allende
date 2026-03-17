using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest
{
    public class 
        
        RequestDocumentChecklistDto
    {
        public int DocumentTypeId { get; set; }

        public string DocumentName { get; set; } = null!;

        public bool RequiredByRule { get; set; }

        public bool NoApplies { get; set; }

        public bool Uploaded { get; set; }

        public string? Observations { get; set; }

        public List<string>? FileNames { get; set; }

        public List<string>? FileUrls { get; set; }

        public List<string>? PreviewUrls { get; set; }
        public List<int> FileIds { get; set; }
    }
}
