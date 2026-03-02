using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands.ToggleRequestDocumentException
{
    public class ToggleRequestDocumentExceptionHandler
       : IRequestHandler<ToggleRequestDocumentExceptionCommand, bool>
    {
        private readonly IRequestDocumentExceptionRepository _repository;
        private readonly ICurrentUserService _currentService;

        public ToggleRequestDocumentExceptionHandler(
            IRequestDocumentExceptionRepository repository,
            ICurrentUserService currentService)
        {
            _repository = repository;
            _currentService = currentService;
        }

        public async Task<bool> Handle(
            ToggleRequestDocumentExceptionCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentService.UserId;

            var existing = await _repository
                .GetByRequestAndDocumentTypeAsync(
                    request.IdRequest,
                    request.IdDocumentType);

            // 🔹 Si se marca como NO aplica
            if (request.DoesNotApply)
            {
                if (existing == null)
                {
                    var entity = new RequestDocumentException
                    {
                        IdRequest = request.IdRequest,
                        IdDocumentType = request.IdDocumentType,
                        DoesNotApply = true,
                        Justification = "Marcado manualmente como no aplica.",
                        CreatedBy = userId,
                        CreatedAt = DateTime.UtcNow,
                        Active = true
                    };

                    await _repository.AddAsync(entity);
                }
                else
                {
                    existing.Active = true;
                    existing.DoesNotApply = true;
                    existing.CreatedBy = userId; // opcional si quieres registrar quién modificó
                    existing.CreatedAt = DateTime.UtcNow;

                    await _repository.UpdateAsync(existing);
                }
            }
            else
            {
                // 🔹 Si se desmarca
                if (existing != null)
                {
                    existing.Active = false;
                    existing.CreatedBy = userId;
                    existing.CreatedAt = DateTime.UtcNow;

                    await _repository.UpdateAsync(existing);
                }
            }

            await _repository.SaveChangesAsync();

            return true;
        }
    }

}