using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.ReviewDocument
{
    public record ReviewDocumentCommand(
     int DocumentId,
     int DocumentStatusId,
     string? ObservationsUpload
 ) : IRequest<bool>;
}
