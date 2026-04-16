using SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Commands.UpdateAcquisitionType;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionClassification_Service.Commands.UpdateAcquisitionClassification
{
    public class UpdateAcqClassificationCmdHandler
        : IRequestHandler<UpdateAcqClassificationCmd, bool>
    {
        private readonly IAcquisitionClassificationRepository _acquisitionRepository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateAcqClassificationCmdHandler(IAcquisitionClassificationRepository acquisitionRepository, ICurrentUserService currentUserService)
        {
            _acquisitionRepository = acquisitionRepository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(
          UpdateAcqClassificationCmd request,
          CancellationToken cancellationToken)
        {
            var acquisitionClass = await _acquisitionRepository
                .GetByIdAsync(request.idUpdateAcquisitionClassification);

            if (acquisitionClass is null)
                return false;

            acquisitionClass.Code = request.Code;
            acquisitionClass.Description = request.Description;
            acquisitionClass.Active = request.Active;
            acquisitionClass.UpdatedBy = _currentUserService.UserId;
            acquisitionClass.UpdatedAt = DateTime.UtcNow;

            return await _acquisitionRepository.UpdateAsync(acquisitionClass);
        }
    }
}
