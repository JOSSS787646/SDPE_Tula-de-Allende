using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Commands.CreateDocumentStatus
{
    public record CreateDocumentStatusCommand(
      int Code,
      string Description,
      int Order
  ) : IRequest<int>;

}
