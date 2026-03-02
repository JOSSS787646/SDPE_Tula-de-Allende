using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest
{
    public class MassiveExpedientDocumentItem
    {
        public int? DocumentTypeId { get; set; }
        public IFormFile File { get; set; } = null!;
        public string? Observations { get; set; }
    }
}
