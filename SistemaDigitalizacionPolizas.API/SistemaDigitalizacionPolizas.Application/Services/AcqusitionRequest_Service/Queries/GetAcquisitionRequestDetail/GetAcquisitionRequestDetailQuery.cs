using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Queries.GetAcquisitionRequestDetail
{
    public record GetAcquisitionRequestDetailQuery(int IdRequest)
       : IRequest<AcquisitionRequestDetailDto?>;
}
