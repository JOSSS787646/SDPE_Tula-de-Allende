using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Queries.GetAcquisitionRequestDetail
{
    public class GetAcquisitionRequestDetailQueryHandler
    : IRequestHandler<GetAcquisitionRequestDetailQuery, AcquisitionRequestDetailDto?>
    {
        private readonly IAcquisitionRequest _repository;

        public GetAcquisitionRequestDetailQueryHandler(IAcquisitionRequest repository)
        {
            _repository = repository;
        }

        public async Task<AcquisitionRequestDetailDto?> Handle(
            GetAcquisitionRequestDetailQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetDetailAsync(request.IdRequest);
        }
    }
}
