using SistemaDigitalizacionPolizas.Domain.Entities.ApplicationStatus;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Commands.CreateDocumentStatus
{
    public class CreateDocumentStatusCommandHandler
        : IRequestHandler<CreateDocumentStatusCommand, int>
    {
        private readonly IDocumentStatusRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public CreateDocumentStatusCommandHandler(
            IDocumentStatusRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(
            CreateDocumentStatusCommand request,
            CancellationToken cancellationToken)
        {
            var entity = new DocumentStatus
            {
                Code = request.Code,
                Description = request.Description,
                Order = request.Order,
                Active = true,
                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _repository.AddAsync(entity);

            if (result == null)
                return 0; // Ya existe un registro con ese Code

            return result.idDocumentStatus;
        }
    }
}
