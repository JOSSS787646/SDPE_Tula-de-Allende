using MediatR;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentType_Service.Commands.CreateDocumentType
{
    public class CreateDocumentTypeCommandHandler
        : IRequestHandler<CreateDocumentTypeCommand, int>
    {
        private readonly IDocumentTypeRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public CreateDocumentTypeCommandHandler(
            IDocumentTypeRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(
            CreateDocumentTypeCommand request,
            CancellationToken cancellationToken)
        {
            var documentType = new DocumentType
            {
                IdDocumentType = request.IdDocumentType,
                DocumentName = request.DocumentName,
                Description = request.Description,
                Active = request.Active,
                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _repository.AddAsync(documentType);

            if (result == null)
                throw new InvalidOperationException(
                    $"Ya existe un tipo de documento con el Id {request.IdDocumentType}");

            return result.IdDocumentType;
        }
    }
}
