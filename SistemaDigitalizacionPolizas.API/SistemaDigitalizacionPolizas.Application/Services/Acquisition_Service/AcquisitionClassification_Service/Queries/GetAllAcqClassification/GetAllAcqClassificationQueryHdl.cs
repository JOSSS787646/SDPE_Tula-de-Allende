using SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Queries.GetAllAcqusitionType;
using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitonType;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionClassification_Service.Queries.GetAllAcqClassification
{
    public class GetAllAcqClassificationQueryHdl
        :IRequestHandler<GetAllAcqClassificationQuery, List<AcquisitionClassificationDto>>
    {
        private readonly IAcquisitionClassificationRepository _acquisitionRepository;

        public GetAllAcqClassificationQueryHdl(IAcquisitionClassificationRepository acquisitionRepository)
        {
            _acquisitionRepository = acquisitionRepository;
        }

        public async Task<List<AcquisitionClassificationDto>> Handle(
      GetAllAcqClassificationQuery request,
      CancellationToken cancellationToken)
        {
            var acquisitionClasses = await _acquisitionRepository.GetAllAsync();

            return acquisitionClasses.Select(acquisitionType => new AcquisitionClassificationDto
            {
                idAcquisitionClassification = acquisitionType.idAcquisitionClassification, 
                Code = acquisitionType.Code,
                Description = acquisitionType.Description,
                Active = acquisitionType.Active
            }).ToList();
        }

    }
}
