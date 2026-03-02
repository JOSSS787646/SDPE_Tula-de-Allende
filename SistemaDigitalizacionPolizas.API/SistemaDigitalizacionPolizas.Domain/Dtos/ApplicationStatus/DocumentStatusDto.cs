using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.ApplicationStatus
{
    public class DocumentStatusDto
    {

        public int idDocumentStatus { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public bool Active { get; set; }
    }
}
