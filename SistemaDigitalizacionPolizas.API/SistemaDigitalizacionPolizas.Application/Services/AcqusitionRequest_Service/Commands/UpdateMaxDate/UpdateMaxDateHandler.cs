using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.UpdateMaxDate
{
    public class UpdateMaxDateHandler : IRequestHandler<UpdateMaxDateCommand, bool>
    {
        private readonly IAcquisitionRequest _repository;

        public UpdateMaxDateHandler(IAcquisitionRequest repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateMaxDateCommand request, CancellationToken cancellationToken)
        {
            if (request.NewMaxDate == default)
                throw new Exception("Fecha inválida");

            await _repository.UpdateMaxDateAsync(request.RequestId, request.NewMaxDate);

            return true;
        }
    }
}
