using SistemaDigitalizacionPolizas.Domain.Dtos.DocumentType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ClasificationDocumentType_Service.Queries.GetDocumentByClassification
{
    public record GetDocumentsByClassificationQuery(
       int AcquisitionClassificationId
   ) : IRequest<List<DocumentAssignmentResponseDto>>;
}
