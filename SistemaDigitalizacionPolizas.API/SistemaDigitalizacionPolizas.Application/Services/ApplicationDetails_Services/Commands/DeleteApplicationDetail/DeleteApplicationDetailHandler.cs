using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ApplicationDetails_Services.Commands.DeleteApplicationDetail
{
    public class DeleteApplicationDetailHandler
    : IRequestHandler<DeleteApplicationDetailCommand>
    {
        private readonly IApplicationDetailRepository _repository;

        public DeleteApplicationDetailHandler(IApplicationDetailRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            DeleteApplicationDetailCommand request,
            CancellationToken cancellationToken)
        {
            // 🔥 Validación
            if (request.IdDetail <= 0)
                throw new Exception("El IdDetail es inválido");

            // 🔍 Verificar existencia
            var entity = await _repository.GetByIdAsync(request.IdDetail);

            if (entity == null)
                throw new Exception("El detalle no existe");

            // ❌ Eliminación física
            await _repository.DeleteAsync(request.IdDetail);

            return Unit.Value;
        }
    }
}
