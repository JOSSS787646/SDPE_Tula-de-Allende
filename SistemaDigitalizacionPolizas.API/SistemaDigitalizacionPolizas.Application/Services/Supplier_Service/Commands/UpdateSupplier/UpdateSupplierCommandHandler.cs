using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Supplier_Service.Commands.UpdateSupplier
{
    public class UpdateSupplierCommandHandler
        : IRequestHandler<UpdateSupplierCommand, bool>
    {
        private readonly ISupplierRepository _repository;

        public UpdateSupplierCommandHandler(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateSupplierCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Supplier;

            var supplier = await _repository.GetByIdAsync(dto.IdSupplier);
            if (supplier is null) return false;

            supplier.Rfc = dto.Rfc.Trim().ToUpper();
            supplier.BusinessName = dto.BusinessName;
            supplier.Street = dto.Street;
            supplier.ExternalNumber = dto.ExternalNumber;
            supplier.InternalNumber = dto.InternalNumber;
            supplier.Neighborhood = dto.Neighborhood;
            supplier.PostalCode = dto.PostalCode;
            supplier.City = dto.City;
            supplier.Municipality = dto.Municipality;
            supplier.State = dto.State;
            supplier.Country = dto.Country;
            supplier.Phone = dto.Phone;
            supplier.ContactName = dto.ContactName;
            supplier.ContactPhone = dto.ContactPhone;
            supplier.Email = dto.Email;
            supplier.Active = dto.Active;

            supplier.UpdatedAt = DateTime.UtcNow;

            return await _repository.UpdateAsync(supplier);
        }
    }
}

