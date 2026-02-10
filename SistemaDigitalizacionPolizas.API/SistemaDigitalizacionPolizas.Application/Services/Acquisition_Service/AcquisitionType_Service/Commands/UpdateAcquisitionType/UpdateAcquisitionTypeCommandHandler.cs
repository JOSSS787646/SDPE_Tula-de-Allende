using MediatR;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Commands.UpdateAcquisitionType
{
    public class UpdateAcquisitionTypeCommandHandler
        : IRequestHandler<UpdateAcquisitionTypeCommand, bool>
    {
        private readonly IAcquisitionRepository _acquisitionTypeRepository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateAcquisitionTypeCommandHandler(
            IAcquisitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _acquisitionTypeRepository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(
            UpdateAcquisitionTypeCommand request,
            CancellationToken cancellationToken)
        {
            var acquisitionType = await _acquisitionTypeRepository
                .GetByIdAsync(request.idUpdateAcquisitionType);

            if (acquisitionType is null)
                return false;

            acquisitionType.Code = request.Code;
            acquisitionType.Description = request.Description;
            acquisitionType.Active = request.Active;
            acquisitionType.UpdatedBy = _currentUserService.UserId;
            acquisitionType.UpdatedAt = DateTime.Now;

            return await _acquisitionTypeRepository.UpdateAsync(acquisitionType);
        }
    }
}
