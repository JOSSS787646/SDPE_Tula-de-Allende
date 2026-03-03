using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands.UpsertRequestDocumentException
{
    public class UpsertRequestDocumentExceptionCommandHandler
        : IRequestHandler<UpsertRequestDocumentExceptionCommand, bool>
    {
        private readonly IRequestDocumentExceptionRepository _repository;

        public UpsertRequestDocumentExceptionCommandHandler(
            IRequestDocumentExceptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpsertRequestDocumentExceptionCommand request,
            CancellationToken cancellationToken)
        {
            var entity = new RequestDocumentException
            {
                IdRequest = request.IdRequest,
                IdDocumentType = request.IdDocumentType,
                DoesNotApply = request.DoesNotApply,
                Active = true,
                Justification = request.Justification,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.UpsertAsync(entity);

            return true;
        }
    }
}
