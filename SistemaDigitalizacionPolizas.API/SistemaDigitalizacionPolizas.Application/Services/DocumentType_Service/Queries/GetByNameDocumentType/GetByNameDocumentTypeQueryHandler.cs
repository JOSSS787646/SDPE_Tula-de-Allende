using SistemaDigitalizacionPolizas.Domain.Dtos.DocumentType;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentType_Service.Queries.GetByNameDocumentType
{
    public class GetByNameDocumentTypeQueryHandler
    : IRequestHandler<GetByNameDocumentTypeQuery, DocumentTypeDto?>
    {
        private readonly IDocumentTypeRepository _repository;

        public GetByNameDocumentTypeQueryHandler(IDocumentTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<DocumentTypeDto?> Handle(
            GetByNameDocumentTypeQuery request,
            CancellationToken cancellationToken)
        {
            var document = await _repository.GetByNameAsync(request.DocumentName);

            if (document == null)
                return null;

            return new DocumentTypeDto
            {
                IdDocumentType = document.IdDocumentType,
                DocumentName = document.DocumentName,
                Description = document.Description,
                Active = document.Active
            };
        }
    }
}
