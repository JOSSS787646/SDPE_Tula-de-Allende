using SistemaDigitalizacionPolizas.Domain.Dtos.ApplicationStatus;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Queries.GetAllDocumentStatus
{
    public class GetAllDocumentStatusQueryHandler
        : IRequestHandler<GetAllDocumentStatusQuery, IEnumerable<DocumentStatusDto>>
    {
        private readonly IDocumentStatusRepository _repository;

        public GetAllDocumentStatusQueryHandler(IDocumentStatusRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DocumentStatusDto>> Handle(
            GetAllDocumentStatusQuery request,
            CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync();

            // 🔁 Mapeo manual a DTO (simple y claro)
            return entities.Select(x => new DocumentStatusDto
            {
                idDocumentStatus = x.idDocumentStatus,
                Code = x.Code,
                Description = x.Description,
                Order = x.Order,
                Active = x.Active
            });
        }
    }
}
