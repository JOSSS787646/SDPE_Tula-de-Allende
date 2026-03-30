using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest
{
    public class UpdateMaxDateDto
    {
        public int RequestId { get; set; }
        public DateTime NewMaxDate { get; set; }
    }
}
