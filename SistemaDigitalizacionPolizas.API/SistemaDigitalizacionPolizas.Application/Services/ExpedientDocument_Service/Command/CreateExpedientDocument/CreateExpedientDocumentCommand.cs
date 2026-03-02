using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.CreateExpedientDocument
{
    public class CreateExpedientDocumentCommand : IRequest<int>
    {
        public int? RequestId { get; set; }
        public int? DocumentTypeId { get; set; }

        // 🔥 FK al catálogo EstadoDocumento
        public int? IdDocumentStatus { get; set; }

        // 📎 Archivo
        public IFormFile File { get; set; } = null!;

        public string? Observations { get; set; }
    }
}
