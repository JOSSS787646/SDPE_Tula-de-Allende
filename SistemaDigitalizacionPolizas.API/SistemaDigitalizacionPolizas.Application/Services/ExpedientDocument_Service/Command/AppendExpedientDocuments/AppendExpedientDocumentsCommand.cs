using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.AppendExpedientDocuments
{
    public record AppendExpedientDocumentsCommand(
         int RequestId,
         int DocumentTypeId,
         List<IFormFile> Files,
         string? Observations
     ) : IRequest<List<int>>; 
}
