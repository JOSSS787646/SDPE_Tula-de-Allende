using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.CreateMassiveExpedientDocument
{
    public record CreateMassiveExpedientDocumentCommand(
     int RequestId,
     List<MassiveExpedientDocumentItem> Documents
 ) : IRequest<List<int>>;
}
