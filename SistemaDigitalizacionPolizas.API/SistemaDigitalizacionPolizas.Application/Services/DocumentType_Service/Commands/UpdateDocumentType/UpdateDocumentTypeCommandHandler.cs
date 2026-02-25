using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentType_Service.Commands.UpdateDocumentType
{
    public class UpdateDocumentTypeCommandHandler
       : IRequestHandler<UpdateDocumentTypeCommand, bool>
    {
        private readonly IDocumentTypeRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public UpdateDocumentTypeCommandHandler(
            IDocumentTypeRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            UpdateDocumentTypeCommand request,
            CancellationToken cancellationToken)
        {
            // 1️⃣ Buscar en BD
            var documentType = await _repository
                .GetByIdAsync(request.IdDocumentType);

            if (documentType == null)
                return false;

            // 2️⃣ Modificar propiedades
            documentType.DocumentName = request.DocumentName;
            documentType.Description = request.Description;
            documentType.Active = request.Active;
            documentType.ModifiedBy = _currentUser.UserId;
            documentType.ModifiedAt = DateTime.UtcNow;

            // 3️⃣ Guardar cambios
            return await _repository.UpdateAsync(documentType);
        }
    }
}
