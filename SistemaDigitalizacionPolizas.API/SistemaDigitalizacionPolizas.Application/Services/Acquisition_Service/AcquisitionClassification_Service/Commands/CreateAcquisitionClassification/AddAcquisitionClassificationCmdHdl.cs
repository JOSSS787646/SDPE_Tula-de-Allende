using SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Commands.CreateAcquisitionType;
using SistemaDigitalizacionPolizas.Domain.Entities.Acquisition_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionClassification_Service.Commands.CreateAcquisitionClassification
{
    public class AddAcquisitionClassificationCmdHdl
        : IRequestHandler<AddAcquisitionClassificationCmd, int>
    {
        private readonly IAcquisitionClassificationRepository _acquisitionRepository;
        private readonly ICurrentUserService _currentUserService;

        public AddAcquisitionClassificationCmdHdl(IAcquisitionClassificationRepository acquisitionRepository, ICurrentUserService currentUserService)
        {
            _acquisitionRepository = acquisitionRepository;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(
     AddAcquisitionClassificationCmd request,
     CancellationToken cancellationToken)
        {
            var acquisitionClass = new AcquisitionClassification
            {
                Code = request.Code,
                Description = request.Description,
                Active = true,
                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _acquisitionRepository.AddAsync(acquisitionClass);

            if (result == null)
                throw new InvalidOperationException(
                    $"Ya existe un tipo de adquisición con el código {request.Code}");

            return result.idAcquisitionClassification;
        }


    }
}
