using MediatR;
using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands.SaveRequestDocumentExceptions
{
    public class ToggleRequestDocumentExceptionMassHandler
        : IRequestHandler<ToggleRequestDocumentExceptionMassCommand, bool>
    {
        private readonly IRequestDocumentExceptionRepository _repository;
        private readonly ICurrentUserService _currentService;

        public ToggleRequestDocumentExceptionMassHandler(
            IRequestDocumentExceptionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentService = currentUserService;
        }

        public async Task<bool> Handle(
            ToggleRequestDocumentExceptionMassCommand request,
            CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var userId = _currentService.UserId;

            var entities = request.Documents
                .Select(doc =>
                {
              
                    return new RequestDocumentException
                    {
                        IdRequest = request.IdRequest,
                        IdDocumentType = doc.IdDocumentType,
                        DoesNotApply = doc.DoesNotApply,
                        Justification = doc.Justification,
                        Active = doc.DoesNotApply,
                        CreatedBy = userId,
                        CreatedAt = now
                    };
                })
                .ToList();

            await _repository.UpsertRangeAsync(entities);

            return true;
        }
    }
}