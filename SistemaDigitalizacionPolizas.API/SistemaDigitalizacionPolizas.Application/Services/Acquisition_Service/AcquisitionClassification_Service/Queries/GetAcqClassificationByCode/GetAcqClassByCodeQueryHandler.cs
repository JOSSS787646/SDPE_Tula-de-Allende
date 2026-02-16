using SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Queries.GetAcquisitionTypeByCode;
using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitonType;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionClassification_Service.Queries.GetAcqClassificationByCode
{
    public class GetAcqClassByCodeQueryHandler
        :IRequestHandler<GetAcqClassByCodeQuery, AcquisitionClassificationDto>
    {
        private readonly IAcquisitionClassificationRepository _acquisitionRepository;
        public GetAcqClassByCodeQueryHandler(IAcquisitionClassificationRepository acquisitionRepository)
        {
            _acquisitionRepository = acquisitionRepository;
        }

        public async Task<AcquisitionClassificationDto> Handle(
  GetAcqClassByCodeQuery request,
  CancellationToken cancellationToken)
        {
            var acquisitionClass = await _acquisitionRepository.GetByCodeAsync(request.Code);

            if (acquisitionClass == null)
                return null;

            return new AcquisitionClassificationDto
            {
                idAcquisitionClassification = acquisitionClass.idAcquisitionClassification,
                Code = acquisitionClass.Code,
                Description = acquisitionClass.Description,
                Active = acquisitionClass.Active
            };
        }
    }
}
