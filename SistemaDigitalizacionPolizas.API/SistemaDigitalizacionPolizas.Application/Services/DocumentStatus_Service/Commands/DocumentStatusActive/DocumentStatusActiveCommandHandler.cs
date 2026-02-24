using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Commands.DocumentStatusActive
{
    public class DocumentStatusActiveCommandHandler
       : IRequestHandler<DocumentStatusActiveCommand, bool>
    {
        private readonly IDocumentStatusRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public DocumentStatusActiveCommandHandler(
            IDocumentStatusRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(
            DocumentStatusActiveCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByCodeAsync(request.Code);

            if (entity == null)
                return false;

            entity.Active = request.Active;
            entity.UpdatedBy = _currentUserService.UserId;
            entity.UpdatedAt = DateTime.UtcNow;

            return await _repository.UpdateAsync(entity);
        }
    }
}
