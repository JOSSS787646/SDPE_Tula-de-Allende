using SistemaDigitalizacionPolizas.Domain.Dtos.ApplicationStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Queries.GetDocumentStatusByCode
{
    public record GetDocumentStatusByCodeQuery(int Code)
     : IRequest<DocumentStatusDto?>;
}
