using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest
{

    //Sirve para enviar la información necesaria para revisar un documento, como el nuevo estado y las observaciones del revisor.
    public class ReviewDocumentRequestDto
    {
        public int DocumentStatusId { get; set; } 
        public string? Observations { get; set; }
    }
}
