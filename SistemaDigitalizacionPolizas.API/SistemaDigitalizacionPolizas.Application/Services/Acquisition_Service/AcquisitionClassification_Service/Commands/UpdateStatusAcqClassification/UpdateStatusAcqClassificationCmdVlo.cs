using SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Commands.UpdateStatusAcquisitionType;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionClassification_Service.Commands.UpdateStatusAcqClassification
{
    public class UpdateStatusAcqClassificationCmdVlo
        : IRequestHandler<UpdateStatusAcqClassificationCmd, bool>
    {
        private readonly IAcquisitionClassificationRepository _acquisitionRepository;
        private readonly ICurrentUserService _currentUserService;
        public UpdateStatusAcqClassificationCmdVlo(IAcquisitionClassificationRepository acquisitionRepository, ICurrentUserService currentUserService)
        {
            _acquisitionRepository = acquisitionRepository;
            _currentUserService = currentUserService;
        }
        public async Task<bool> Handle(
UpdateStatusAcqClassificationCmd request,
CancellationToken cancellationToken)
        {
            var acquisitionClass = await _acquisitionRepository.GetByCodeAsync(request.Code);

            if (acquisitionClass == null)
                return false;
            acquisitionClass.Active = request.Active;
            acquisitionClass.UpdatedBy = _currentUserService.UserId;
            acquisitionClass.UpdatedAt = DateTime.UtcNow;

            return await _acquisitionRepository.UpdateAsync(acquisitionClass);
        }
    }
}
