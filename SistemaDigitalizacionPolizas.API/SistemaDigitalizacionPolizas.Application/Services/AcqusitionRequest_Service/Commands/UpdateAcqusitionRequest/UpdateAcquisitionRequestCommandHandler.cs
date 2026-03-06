using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.UpdateAcqusitionRequest
{
    public class UpdateAcquisitionRequestCommandHandler
        : IRequestHandler<UpdateAcquisitionRequestCommand, bool>
    {
        private readonly IAcquisitionRequest _repository;

        public UpdateAcquisitionRequestCommandHandler(
            IAcquisitionRequest repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateAcquisitionRequestCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var entity = await _repository.GetByIdAsync(dto.IdRequest);

            if (entity == null)
                return false;

            entity.RequestNumber = dto.RequestNumber;
            entity.RequestDate = dto.RequestDate;
            entity.Justification = dto.Justification;
            entity.AuthorizationDate = dto.AuthorizationDate;
            entity.Observations = dto.Observations;

            entity.IdAdministrativeUnit = dto.IdAdministrativeUnit;
            entity.IdProject = dto.IdProject;
            entity.IdAcquisitionType = dto.IdAcquisitionType;
            entity.IdSupplier = dto.IdSupplier;
            entity.IdFundingSource = dto.IdFundingSource;
            entity.IdAcquisitionClassification = dto.IdAcquisitionClassification;
            entity.IdProgram = dto.IdProgram;
            entity.IdCommunity = dto.IdCommunity;
            entity.IdBeneficiary = dto.IdBeneficiary;
            entity.IdPaymentPolicy = dto.IdPolicy;

            entity.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(entity);

            return true;
        }
    }
}
