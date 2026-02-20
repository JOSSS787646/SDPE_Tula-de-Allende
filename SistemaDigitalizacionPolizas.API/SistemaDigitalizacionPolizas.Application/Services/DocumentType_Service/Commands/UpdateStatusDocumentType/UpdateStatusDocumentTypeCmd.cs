using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentType_Service.Commands.UpdateStatusDocumentType
{
    public record UpdateStatusDocumentTypeCmd
        (string DocumentName,
    bool Active)
        : IRequest<bool>;

}
