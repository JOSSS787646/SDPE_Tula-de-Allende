using SistemaDigitalizacionPolizas.Application.Services.Community_Service.Queries.GetAllCommunity;
using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitonType;
using SistemaDigitalizacionPolizas.Domain.Dtos.Community;
using SistemaDigitalizacionPolizas.Domain.Entities.Acquisition_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Queries.GetAllAcqusitionType
{
    public class GetAllAcquisitionTypeQueryHandler
        : IRequestHandler<GetAllAcquisitionTypeQuery, List<AcquisitionTypeDto>>
    {
        private readonly IAcquisitionRepository _acquisitionRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetAllAcquisitionTypeQueryHandler(IAcquisitionRepository acquisitionRepository, ICurrentUserService currentUserService)
        {
            _acquisitionRepository = acquisitionRepository;
            _currentUserService = currentUserService;
        }

        public async Task<List<AcquisitionTypeDto>> Handle(
GetAllAcquisitionTypeQuery request,
CancellationToken cancellationToken)
        {
            var acquisitionTypes = await _acquisitionRepository.GetAllAsync();

            return acquisitionTypes.Select(acquisitionType => new AcquisitionTypeDto
            {
                idAcquisitionType = acquisitionType.idAcquisitionType,
                Code = acquisitionType.Code,
                Description = acquisitionType.Description,
                Active = acquisitionType.Active
            }).ToList();
        }
    }
}
