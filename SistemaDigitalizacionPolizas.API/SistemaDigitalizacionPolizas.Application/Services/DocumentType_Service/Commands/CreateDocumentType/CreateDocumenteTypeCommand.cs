using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentType_Service.Commands.CreateDocumentType
{
    public record CreateDocumentTypeCommand
 (
     int IdDocumentType,
     string DocumentName,
     string Description,
     bool IsRequired,
     bool Active
 ) : IRequest<int>;
}
