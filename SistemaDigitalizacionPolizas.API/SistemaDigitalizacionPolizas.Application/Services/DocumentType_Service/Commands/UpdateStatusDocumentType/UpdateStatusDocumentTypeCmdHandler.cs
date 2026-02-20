using MediatR;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentType_Service.Commands.UpdateStatusDocumentType
{
    public class UpdateStatusDocumentTypeCmdHandler
        : IRequestHandler<UpdateStatusDocumentTypeCmd, bool>
    {
        private readonly IDocumentTypeRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public UpdateStatusDocumentTypeCmdHandler(
            IDocumentTypeRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
    UpdateStatusDocumentTypeCmd request,
    CancellationToken cancellationToken)
        {
            var document = await _repository
                .GetByNameAsync(request.DocumentName);

            if (document == null)
                return false;

            document.Active = request.Active; // ← aquí asignas directamente
            document.ModifiedBy = _currentUser.UserId;
            document.ModifiedAt = DateTime.UtcNow;

            return await _repository.UpdateAsync(document);
        }
    }
}
