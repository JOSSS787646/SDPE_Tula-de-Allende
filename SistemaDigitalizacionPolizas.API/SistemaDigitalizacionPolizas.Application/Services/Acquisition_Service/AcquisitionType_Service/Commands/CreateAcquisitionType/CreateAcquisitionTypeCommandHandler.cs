using SistemaDigitalizacionPolizas.Application.Services.Community_Service.Commands.CreateCommunity;
using SistemaDigitalizacionPolizas.Domain.Entities.Acquisition_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Acquisition_Service.AcquisitionType_Service.Commands.CreateAcquisitionType
{
    public class CreateAcquisitionTypeCommandHandler
        : IRequestHandler<CreateAcquisitionTypeCommand, int>
    {
        private readonly IAcquisitionRepository _acquisitionRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateAcquisitionTypeCommandHandler(IAcquisitionRepository acquisitionRepository, ICurrentUserService currentUserService)
        {
            _acquisitionRepository = acquisitionRepository;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(
        CreateAcquisitionTypeCommand request,
        CancellationToken cancellationToken)
        {
            var acquisitionType = new AcquisitionType
            {
                Code = request.Code,
                Description = request.Description,
                Active = true,
                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTime.Now
            };

            var result = await _acquisitionRepository.AddAsync(acquisitionType);

            if (result == null)
                throw new InvalidOperationException(
                    $"Ya existe un tipo de adquisicion con el código {request.Code}");

            return result.idAcquisitionType;
        }
    }
}
