using SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Queries.GetRequiredDocumentsByRequest
{
    public record GetDocumentsByAcquisitionClassificationQuery(int AcquisitionClassificationId)
      : IRequest<IEnumerable<ExpedientDocumentPreviewDto>>;
}

