using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.UpdateCFDI
{
    public class UpdateCFDICommandHanlder
        :IRequestHandler<UpdateCFDICommand, bool>
    {

        private readonly IAcquisitionRequest _repository;

        public UpdateCFDICommandHanlder(IAcquisitionRequest repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateCFDICommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.IdRequest);

            if (entity == null)
                throw new Exception("La solicitud no existe");

            // 🔹 solo actualizamos la póliza
            entity.CFDI = request.CFDI;

            await _repository.UpdateAsync(entity);

            return true;
        }
    }
}
