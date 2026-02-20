using SistemaDigitalizacionPolizas.Domain.Dtos.DocumentType;
using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ClasificationDocumentType_Service.Queries.GetDocumentByClassification
{
    public class GetDocumentsByClassificationQueryHandler
        : IRequestHandler<GetDocumentsByClassificationQuery, List<DocumentAssignmentResponseDto>>
    {
        private readonly IClasificationDocumentTypeRepository _repository;

        public GetDocumentsByClassificationQueryHandler(
            IClasificationDocumentTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DocumentAssignmentResponseDto>> Handle(
            GetDocumentsByClassificationQuery request,
            CancellationToken cancellationToken)
        {
            var entities = await _repository
                .GetByClassificationAsync(request.AcquisitionClassificationId);

            var result = entities.Select(x => new DocumentAssignmentResponseDto
            {
                DocumentTypeId = x.DocumentTypeId,
                DocumentName = x.DocumentType.DocumentName,
                IsRequired = x.IsRequired,
                Active = x.Active
            }).ToList();

            return result;
        }
    }
}
