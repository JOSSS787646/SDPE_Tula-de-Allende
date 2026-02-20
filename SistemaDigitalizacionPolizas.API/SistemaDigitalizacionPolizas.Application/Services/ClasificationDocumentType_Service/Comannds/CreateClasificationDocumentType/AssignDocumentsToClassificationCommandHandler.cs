using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ClasificationDocumentType_Service.Comannds.CreateClasificationDocumentType
{
    public class AssignDocumentsToClassificationCommandHandler
        : IRequestHandler<AssignDocumentsToClassificationCommand, bool>
    {
        private readonly IClasificationDocumentTypeRepository _repository;

        public AssignDocumentsToClassificationCommandHandler(
            IClasificationDocumentTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AssignDocumentsToClassificationCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Documents == null || !request.Documents.Any())
                return false;

            var entities = request.Documents.Select(d =>
                new ClasificationDocumentType
                {
                    ClassificationAcquisitionId = request.AcquisitionClassificationId,
                    DocumentTypeId = d.DocumentTypeId,
                    IsRequired = d.IsRequired,
                    Active = d.Active
                });

            await _repository.ReplaceAsync(
                request.AcquisitionClassificationId,
                entities);

            return true;
        }
    }
}
