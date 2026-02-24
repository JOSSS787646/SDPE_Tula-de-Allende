using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SSistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AppliccationStatus_Service.Commands.UpdateApplicationStatus
{
    public class UpdateApplicationStatusCommandHandler
       : IRequestHandler<UpdateApplicationStatusCommand, bool>
    {
        private readonly IApplicationStatusRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateApplicationStatusCommandHandler(
            IApplicationStatusRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(
            UpdateApplicationStatusCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.IdApplicationStatus);

            if (entity == null)
                throw new Exception("Estado no encontrado.");

            // 🔥 Actualización controlada
            entity.Description = request.Description;
            entity.Order = request.Order;
            entity.Active = request.Active;

            // 🔐 Auditoría automática
            entity.UpdatedBy = _currentUserService.UserId;
            entity.UpdatedAt = DateTime.Now;

            return await _repository.UpdateAsync(entity);
        }
    }
}
