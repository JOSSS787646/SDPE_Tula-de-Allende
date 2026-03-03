using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Queries.GetAllAcquisitionRequest
{
    public class GetAllAcquisitionRequestPolizaCommandHandler
       : IRequestHandler<GetAllAcquisitionRequestPolizaCommand, IEnumerable<AcquisitionRequestPolizaDto>>
    {
        private readonly IAcquisitionRequest _repository;

        public GetAllAcquisitionRequestPolizaCommandHandler(
            IAcquisitionRequest repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AcquisitionRequestPolizaDto>> Handle(
            GetAllAcquisitionRequestPolizaCommand request,
            CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
            var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            var result = await _repository
                .GetAllPolizaInfoPaginatedAsync(pageNumber, pageSize);

            return result.Data; // 🔥 solo la lista
        }
    }
}
