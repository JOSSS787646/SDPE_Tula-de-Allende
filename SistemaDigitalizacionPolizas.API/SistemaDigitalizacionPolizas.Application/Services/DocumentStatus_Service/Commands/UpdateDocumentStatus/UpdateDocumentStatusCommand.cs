using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Commands.UpdateDocumentStatus
{
    public record UpdateDocumentStatusCommand(
        int IdDocumentStatus,   
        string Description,
        int Order,
        bool Active
    ) : IRequest<bool>;
}
