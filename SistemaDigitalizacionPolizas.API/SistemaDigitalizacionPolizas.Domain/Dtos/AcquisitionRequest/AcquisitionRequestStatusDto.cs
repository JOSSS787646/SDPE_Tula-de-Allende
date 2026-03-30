using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest
{
    public class AcquisitionRequestStatusDto
    {
        public int IdRequest { get; set; }
        public int? IdAcquisitionClassification { get; set; }
        public int? IdApplicationStatus { get; set; }
        public DateTime? CompleteMaximeDate { get; set; }
    }
}
