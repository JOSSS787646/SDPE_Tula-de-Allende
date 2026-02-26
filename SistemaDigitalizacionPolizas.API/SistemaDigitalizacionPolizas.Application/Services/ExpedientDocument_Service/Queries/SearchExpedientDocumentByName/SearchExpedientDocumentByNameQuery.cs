using SistemaDigitalizacionPolizas.Domain.Dtos.ExpedientDocument;
using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Queries.SearchExpedientDocumentByName
{
    public record SearchExpedientDocumentByNameQuery(
     int RequestId,
     string FileName
 ) : IRequest<List<ExpedientDocumentSearchDto>>;
}
