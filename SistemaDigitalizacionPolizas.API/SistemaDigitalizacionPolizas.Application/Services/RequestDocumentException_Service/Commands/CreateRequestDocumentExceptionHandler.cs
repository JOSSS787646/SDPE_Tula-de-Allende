using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands
{
    public class CreateRequestDocumentExceptionHandler
        : IRequestHandler<CreateRequestDocumentExceptionCommand, int>
    {
        private readonly IRequestDocumentExceptionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public CreateRequestDocumentExceptionHandler(
            IRequestDocumentExceptionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(
            CreateRequestDocumentExceptionCommand request,
            CancellationToken cancellationToken)
        {
            // 🔎 Validación básica
            if (string.IsNullOrWhiteSpace(request.Justification))
                throw new ArgumentException("La justificación es obligatoria.");

            var userId = _currentUserService.UserId;

            var entity = new RequestDocumentException
            {
                IdRequest = request.IdRequest,
                IdDocumentType = request.IdDocumentType,
                DoesNotApply = request.DoesNotApply,
                Justification = request.Justification,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow,
                Active = true
            };

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            return entity.IdRequestDocumentException;
        }
    }
}
