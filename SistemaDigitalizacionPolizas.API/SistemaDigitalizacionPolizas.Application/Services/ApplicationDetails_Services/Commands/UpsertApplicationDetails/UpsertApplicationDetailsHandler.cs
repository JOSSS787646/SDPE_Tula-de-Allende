using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ApplicationDetails_Services.Commands.UpsertApplicationDetails
{
    public class UpsertApplicationDetailsHandler
     : IRequestHandler<UpsertApplicationDetailsCommand>
    {
        private readonly IApplicationDetailRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public UpsertApplicationDetailsHandler(
            IApplicationDetailRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(
            UpsertApplicationDetailsCommand request,
            CancellationToken cancellationToken)
        {
            // ==========================================
            // 🔥 VALIDACIÓN
            // ==========================================
            if (request.Details == null || !request.Details.Any())
                throw new Exception("Debe enviar al menos un detalle.");

            var userId = _currentUser.UserId;

            // ==========================================
            // 🔄 MAPEO + LÓGICA
            // ==========================================
            var entities = request.Details.Select(d =>
            {
                if (d.Quantity <= 0)
                    throw new Exception("La cantidad debe ser mayor a 0");

                if (d.UnitAmount < 0)
                    throw new Exception("El importe no puede ser negativo");

                if (d.Cog == null)
                    throw new Exception("El COG es obligatorio");

                return new ApplicationDetail
                {
                    IdDetail = d.IdDetail,
                    ApplicationId = request.RequestId,

                    CogId = d.Cog.Id,

                    Quantity = d.Quantity,
                    UnitMeasure = d.UnitMeasure,
                    Description = d.Description,

                    UnitAmount = d.UnitAmount,

                    // 🔥 cálculo SIEMPRE backend
                    TotalAmount = d.Quantity * d.UnitAmount,

                    // 🔥 auditoría
                    CreatedBy = d.IdDetail == 0 ? userId : null,
                    CreatedDate = d.IdDetail == 0 ? DateTime.UtcNow : default,

                    ModifiedBy = d.IdDetail != 0 ? userId : null,
                    ModifiedDate = d.IdDetail != 0 ? DateTime.UtcNow : null,

                    Active = true
                };
            }).ToList();

            // ==========================================
            // 💾 PERSISTENCIA
            // ==========================================
            await _repository.UpsertRangeAsync(request.RequestId, entities);

            return Unit.Value;
        }
    }
}
