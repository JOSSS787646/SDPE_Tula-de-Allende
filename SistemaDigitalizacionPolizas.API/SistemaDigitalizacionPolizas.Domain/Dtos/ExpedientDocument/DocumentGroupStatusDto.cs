using SistemaDigitalizacionPolizas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument
{

    public class DocumentGroupStatusDto
    {
        public int DocumentTypeId { get; set; }
        public string DocumentName { get; set; }

        public int TotalRequired { get; set; }
        public int TotalUploaded { get; set; }
        public int TotalApproved { get; set; }

        public DocumentGroupStatus Status { get; set; }

        // 🔥 Solo texto (sin extensión)
        public string StatusLabel => Status.ToString();
    }
}

