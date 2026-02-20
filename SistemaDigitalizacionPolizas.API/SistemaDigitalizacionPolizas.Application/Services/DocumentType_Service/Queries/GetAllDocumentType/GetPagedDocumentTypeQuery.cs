using SistemaDigitalizacionPolizas.Domain.Dtos.DocumentType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentType_Service.Queries.GetAllDocumentType
{
    public record GetPagedDocumentTypeQuery(
        int PageNumber,
        int PageSize
    ) : IRequest<IEnumerable<DocumentTypeDto>>;
}
