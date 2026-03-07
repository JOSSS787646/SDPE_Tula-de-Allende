using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.DeleteExpedientDocument
{
    public record DeleteExpedientDocumentCommand(
    int Id,
    string Password
) : IRequest<bool>;
}
