using SistemaDigitalizacionPolizas.Application.Services.Community_Service.Queries.GetCommunityByCode;
using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitonType;
using SistemaDigitalizacionPolizas.Domain.Dtos.Community;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Queries.GetAcquisitionTypeByCode
{
    public class GetAcquisitionTypeByCodeQueryHandler
            : IRequestHandler<GetAcquisitionTypeByCodeQuery, AcquisitionTypeDto>
    {
        private readonly IAcquisitionRepository _acquisitionRepository;
        public GetAcquisitionTypeByCodeQueryHandler(IAcquisitionRepository acquisitionRepository)
        {
            _acquisitionRepository = acquisitionRepository;
        }
        public async Task<AcquisitionTypeDto> Handle(
      GetAcquisitionTypeByCodeQuery request,
      CancellationToken cancellationToken)
        {
            var acquisitionTypes = await _acquisitionRepository.GetByCodeAsync(request.Code);

            if (acquisitionTypes == null)
                return null;

            return new AcquisitionTypeDto
            {
                idAcquisitionType = acquisitionTypes.idAcquisitionType,
                Code = acquisitionTypes.Code,
                Description = acquisitionTypes.Description,
                Active = acquisitionTypes.Active
            };
        }
    }
}
