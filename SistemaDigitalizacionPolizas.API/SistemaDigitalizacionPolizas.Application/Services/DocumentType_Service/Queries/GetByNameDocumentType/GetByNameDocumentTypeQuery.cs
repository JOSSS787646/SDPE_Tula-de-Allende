using SistemaDigitalizacionPolizas.Domain.Dtos.DocumentType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentType_Service.Queries.GetByNameDocumentType
{
    public record GetByNameDocumentTypeQuery(string DocumentName)
    : IRequest<DocumentTypeDto?>;

}
