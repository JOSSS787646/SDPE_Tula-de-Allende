using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.UpdatePaymentPolicy
{
    public class UpdatePaymentPolicyCommandHandler
        : IRequestHandler<UpdatePaymentPolicyCommand, bool>
    {
        private readonly IAcquisitionRequest _repository;

        public UpdatePaymentPolicyCommandHandler(IAcquisitionRequest repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdatePaymentPolicyCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.IdRequest);

            if (entity == null)
                throw new Exception("La solicitud no existe");

            // 🔹 solo actualizamos la póliza
            entity.IdPaymentPolicy = request.IdPaymentPolicy;

            await _repository.UpdateAsync(entity);

            return true;
        }
    }
}
