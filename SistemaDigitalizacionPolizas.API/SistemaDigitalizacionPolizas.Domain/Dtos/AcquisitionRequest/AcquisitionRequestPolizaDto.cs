using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest
{
    public class AcquisitionRequestPolizaDto
    {
        // Número de folio
        public string? Folio { get; set; }

        // Id de la solicitud
        public int IdRequest { get; set; }

        // Tipo de clasificación de adquisición
        public string? AcquisitionClassification { get; set; }

        // Fecha en que fue expedida
        public DateTime? RequestDate { get; set; }

        // Estatus actual
        public string? Status { get; set; }

        // Número de póliza (aún no existe)
        public string? PolicyNumber { get; set; }
    }
}
