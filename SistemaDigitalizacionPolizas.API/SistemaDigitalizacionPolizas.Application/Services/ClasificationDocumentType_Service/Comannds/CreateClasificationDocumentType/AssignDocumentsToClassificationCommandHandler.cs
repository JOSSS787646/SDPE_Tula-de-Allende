using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service;
using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
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
        private readonly IAcquisitionRequest _requestRepository; // 👈 necesitas este
        private readonly IRequestStatusService _requestStatusService;
        private readonly IUnitOfWorkService _unitOfWork;

        public AssignDocumentsToClassificationCommandHandler(
            IClasificationDocumentTypeRepository repository,
            IAcquisitionRequest requestRepository,
            IRequestStatusService requestStatusService,
            IUnitOfWorkService unitOfWork)
        {
            _repository = repository;
            _requestRepository = requestRepository;
            _requestStatusService = requestStatusService;
            _unitOfWork = unitOfWork;
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
                    IsRequired = true,
                    Active = d.Active
                }).ToList();

            // ====================================================
            // 🔹 TRANSACCIÓN 1: UPSERT
            // ====================================================
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _repository.UpsertRangeAsync(
                    request.AcquisitionClassificationId,
                    entities);

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            // ====================================================
            // 🔹 TRANSACCIÓN 2: RECALCULAR ESTADOS
            // ====================================================
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 🔥 obtener solicitudes afectadas por la clasificación
                var requests = await _requestRepository
                    .GetByClassificationId(request.AcquisitionClassificationId);

                foreach (var req in requests)
                {
                    await _requestStatusService.RecalculateStatus(req.IdRequest);
                }

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            return true;
        }
    }
}
