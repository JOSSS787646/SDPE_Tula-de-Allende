using MediatR;
using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.CreateRequest
{
    public class CreateRequestCommandHandler
       : IRequestHandler<CreateRequestCommand, int>
    {
        private readonly IAcquisitionRequest _repository;
        private readonly ICurrentUserService _currentUser;

        public CreateRequestCommandHandler(
            IAcquisitionRequest repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<int> Handle(
            CreateRequestCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var entity = new AcquisitionRequest
            {
                // ===============================
                // Información del Negocio
                // ===============================

                RequestNumber = dto.RequestNumber,
                RequestDate = dto.RequestDate,
                Justification = dto.Justification,
                AuthorizationDate = dto.AuthorizationDate,
                Observations = dto.Observations,

                // ===============================
                // Foreign Keys (Nullable)
                // ===============================

                IdAdministrativeUnit = dto.IdAdministrativeUnit,
                IdProject = dto.IdProject,
                IdAcquisitionType = dto.IdAcquisitionType,
                IdSupplier = dto.IdSupplier,
                IdApplicationStatus = dto.IdApplicationStatus,
                IdFundingSource = dto.IdFundingSource,
                IdAcquisitionClassification = dto.IdAcquisitionClassification,
                IdProgram = dto.IdProgram,
                IdCommunity = dto.IdCommunity,
                IdBeneficiary = dto.IdBeneficiary,

                // ===============================
                // Auditoría
                // ===============================

                CreatedBy = _currentUser.UserId,
                CreatedAt = DateTime.Now,
                Active = true
            };

            var result = await _repository.AddAsync(entity);

            return result!.IdRequest;
        }
    }
}
