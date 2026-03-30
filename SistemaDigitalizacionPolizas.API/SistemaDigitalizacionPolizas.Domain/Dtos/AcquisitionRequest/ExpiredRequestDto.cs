using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest
{
    public class ExpiredRequestDto
    {
        public int RequestId { get; set; }

        public string RequestNumber { get; set; } = string.Empty;

        public string UserEmail { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public DateTime? LastNotificationSentAt { get; set; }

        // 🔥 Documentos faltantes (sin depender de otro DTO)
        public List<MissingDocument> MissingDocuments { get; set; } = new();

        public class MissingDocument
        {
            public string DocumentName { get; set; } = string.Empty;

            public string Status { get; set; } = string.Empty;
        }
    }
}
