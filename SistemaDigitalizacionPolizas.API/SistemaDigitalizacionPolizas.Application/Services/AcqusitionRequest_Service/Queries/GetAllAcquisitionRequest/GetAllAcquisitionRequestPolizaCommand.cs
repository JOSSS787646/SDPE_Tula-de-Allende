using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Queries.GetAllAcquisitionRequest
{


    public class GetAllAcquisitionRequestPolizaCommand
  : IRequest<IEnumerable<AcquisitionRequestPolizaDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllAcquisitionRequestPolizaCommand(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

}
