using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.UpdateExpedientDocument
{
    public class UpdateExpedientDocumentCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public string? Observations { get; set; }

        public IFormFile? NewFile { get; set; }
    }
}
