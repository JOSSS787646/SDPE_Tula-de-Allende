using SistemaDigitalizacionPolizas.Domain.Entities.ApplicationStatus;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SSistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AppliccationStatus_Service.Commands.CreateApplicationStatus
{
    public class CreateApplicationStatusCommandHandler
    : IRequestHandler<CreateApplicationStatusCommand, int>
    {
        private readonly IApplicationStatusRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public CreateApplicationStatusCommandHandler(
            IApplicationStatusRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(
            CreateApplicationStatusCommand request,
            CancellationToken cancellationToken)
        {
            var entity = new ApplicationStatus
            {
                Code = request.Code,
                Description = request.Description,
                Order = request.Order,
                CreatedBy = _currentUserService.UserId,
                Active = true
            };

            var result = await _repository.AddAsync(entity);

            if (result == null)
                throw new Exception("Ya existe un estado con ese código.");

            return result.IdApplicationStatus;
        }
    }

}
