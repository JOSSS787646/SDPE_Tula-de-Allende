using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AppliccationStatus_Service.Commands.UpdateApplicationStatus
{
    public class UpdateApplicationStatusStateByCodeCommandHandler
       : IRequestHandler<UpdateApplicationStatusStateCommand, bool>
    {
        private readonly IApplicationStatusRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateApplicationStatusStateByCodeCommandHandler(
            IApplicationStatusRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(
            UpdateApplicationStatusStateCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByCodeAsync(request.Code);

            if (entity == null)
                return false;

            entity.Active = request.Active;
            entity.UpdatedBy = _currentUserService.UserId;
            entity.UpdatedAt = DateTime.Now;

            return await _repository.UpdateAsync(entity);
        }
    }
}
