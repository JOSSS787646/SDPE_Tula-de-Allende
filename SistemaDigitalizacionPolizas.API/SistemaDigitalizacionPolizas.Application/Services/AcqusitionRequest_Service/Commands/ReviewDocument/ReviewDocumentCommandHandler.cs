using MediatR;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service;
using SistemaDigitalizacionPolizas.Domain.Enums;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.ReviewDocument
{
    public class ReviewDocumentCommandHandler
        : IRequestHandler<ReviewDocumentCommand, bool>
    {
        private readonly IDocumentExpedientRepository _repository;
        private readonly IRequestStatusService _requestStatusService;
        private readonly IUnitOfWorkService _unitOfWork;

        public ReviewDocumentCommandHandler(
            IDocumentExpedientRepository repository,
            IRequestStatusService requestStatusService,
            IUnitOfWorkService unitOfWork)
        {
            _repository = repository;
            _requestStatusService = requestStatusService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            ReviewDocumentCommand request,
            CancellationToken cancellationToken)
        {
            var document = await _repository.GetByIdAsync(request.DocumentId);

            if (document == null)
                throw new Exception("Documento no encontrado.");

            if (request.DocumentStatusId == (int)DocumentStatusEnum.Observado &&
                string.IsNullOrWhiteSpace(request.Observations))
            {
                throw new Exception("Debe agregar observaciones cuando el documento es observado.");
            }

            document.IdDocumentStatus = request.DocumentStatusId;
            document.Observations = request.Observations;

            await _repository.UpdateAsync(document);

            await _requestStatusService.RecalculateStatus(document.RequestId);



            return true;
        }
    }
}