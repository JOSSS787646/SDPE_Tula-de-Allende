using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Commands.UpdateDocumentStatus
{
    public class UpdateDocumentStatusCommandHandler
      : IRequestHandler<UpdateDocumentStatusCommand, bool>
    {
        private readonly IDocumentStatusRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateDocumentStatusCommandHandler(
            IDocumentStatusRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(
            UpdateDocumentStatusCommand request,
            CancellationToken cancellationToken)
        {
            // 🔍 Buscar por ID real (no por Code)
            var entity = await _repository.GetByIdAsync(request.IdDocumentStatus);

            if (entity == null)
                return false;

            entity.Description = request.Description;
            entity.Order = request.Order;
            entity.Active = request.Active;

            entity.UpdatedBy = _currentUserService.UserId;
            entity.UpdatedAt = DateTime.UtcNow;

            return await _repository.UpdateAsync(entity);
        }
    }
}
