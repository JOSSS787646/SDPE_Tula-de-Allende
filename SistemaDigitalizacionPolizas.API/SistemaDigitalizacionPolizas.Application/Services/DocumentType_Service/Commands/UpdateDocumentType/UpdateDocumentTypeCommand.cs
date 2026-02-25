using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentType_Service.Commands.UpdateDocumentType
{
    public record UpdateDocumentTypeCommand(
     int IdDocumentType,
     string DocumentName,
     string Description,
     bool Active
 ) : IRequest<bool>;
}
