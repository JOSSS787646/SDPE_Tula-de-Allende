using MediatR;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.UpdateMaxDate
{
    public class UpdateMaxDateHandler : IRequestHandler<UpdateMaxDateCommand, bool>
    {
        private readonly IAcquisitionRequest _repository;
        private readonly IRequestStatusService _requestStatusService;
        private readonly IUnitOfWorkService _unitOfWork; 

        public UpdateMaxDateHandler(
            IAcquisitionRequest repository,
            IRequestStatusService requestStatusService,
            IUnitOfWorkService unitOfWork) 
        {
            _repository = repository;
            _requestStatusService = requestStatusService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateMaxDateCommand request, CancellationToken cancellationToken)
        {
            if (request.NewMaxDate == default)
                throw new Exception("Fecha inválida");

            await _repository.UpdateMaxDateAsync(request.RequestId, request.NewMaxDate);
            await _unitOfWork.SaveChangesAsync();
            await _requestStatusService.RecalculateStatus(request.RequestId);

            return true;
        }
    }
}