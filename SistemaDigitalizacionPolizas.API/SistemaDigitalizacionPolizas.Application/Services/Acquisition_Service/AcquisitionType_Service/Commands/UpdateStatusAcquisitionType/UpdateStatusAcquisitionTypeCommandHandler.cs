using SistemaDigitalizacionPolizas.Application.Services.Community_Service.Commands.UpdateStatusCommunity;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Commands.UpdateStatusAcquisitionType
{
    public class UpdateStatusAcquisitionTypeCommandHandler
        : IRequestHandler<UpdateStatusAcquisitionTypeCommand, bool>
    {
        private readonly IAcquisitionRepository _acquisitionRepository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateStatusAcquisitionTypeCommandHandler(IAcquisitionRepository acquisitionRepository, ICurrentUserService currentUserService)
        {
            _acquisitionRepository = acquisitionRepository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(
UpdateStatusAcquisitionTypeCommand request,
CancellationToken cancellationToken)
        {
            var acquisitionType = await _acquisitionRepository.GetByCodeAsync(request.Code);

            if (acquisitionType == null)
                return false;
            acquisitionType.Active = request.Active;
            acquisitionType.UpdatedBy = _currentUserService.UserId;
            acquisitionType.UpdatedAt = DateTime.Now;

            return await _acquisitionRepository.UpdateAsync(acquisitionType);
        }
    }
}
