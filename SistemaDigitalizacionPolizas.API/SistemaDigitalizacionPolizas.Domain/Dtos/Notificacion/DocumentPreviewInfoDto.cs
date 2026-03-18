using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion
{
    public class DocumentPreviewInfoDto
    {
        public string RequestNumber { get; set; }
        public string AdministrativeUnit { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public List<DocumentItemDto> Documents { get; set; }
    }

    public class DocumentItemDto
    {
        public string FileName { get; set; }
        public string Status { get; set; }
        public string? Observations { get; set; }
    }
}
