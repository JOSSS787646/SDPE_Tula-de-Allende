using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Supplier_Service.Commands.UpdateSupplierStatus
{
    public class UpdateSupplierStatusCommandHandler
        : IRequestHandler<UpdateSupplierStatusCommand, bool>
    {
        private readonly ISupplierRepository _repository;

        public UpdateSupplierStatusCommandHandler(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
    UpdateSupplierStatusCommand request,
    CancellationToken cancellationToken)
        {
            var supplier = await _repository.GetByRfcAsync(request.RfcSupplier);

            if (supplier is null)
                return false;

            supplier.Active = request.Active;
            supplier.UpdatedAt = DateTime.UtcNow;

            return await _repository.UpdateAsync(supplier);
        }

    }
}
