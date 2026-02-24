using SistemaDigitalizacionPolizas.Domain.Dtos.ApplicationStatus;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.DocumentStatus_Service.Queries.GetDocumentStatusByCode
{
    public class GetDocumentStatusByCodeQueryHandler
       : IRequestHandler<GetDocumentStatusByCodeQuery, DocumentStatusDto?>
    {
        private readonly IDocumentStatusRepository _repository;

        public GetDocumentStatusByCodeQueryHandler(IDocumentStatusRepository repository)
        {
            _repository = repository;
        }

        public async Task<DocumentStatusDto?> Handle(
            GetDocumentStatusByCodeQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByCodeAsync(request.Code);

            if (entity == null)
                return null;

            return new DocumentStatusDto
            {
                idDocumentStatus = entity.idDocumentStatus,
                Code = entity.Code,
                Description = entity.Description,
                Order = entity.Order,
                Active = entity.Active
            };
        }
    }
}
