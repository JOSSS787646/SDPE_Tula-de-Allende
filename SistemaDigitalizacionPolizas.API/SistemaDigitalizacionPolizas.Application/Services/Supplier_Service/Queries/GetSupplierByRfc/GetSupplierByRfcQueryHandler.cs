using SistemaDigitalizacionPolizas.Domain.Dtos.Supplier;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Supplier_Service.Queries.GetSupplierByRfc
{

    public class GetSupplierByRfcQueryHandler
        : IRequestHandler<GetSupplierByRfcQuery, SupplierDto?>
    {
        private readonly ISupplierRepository _repository;

        public GetSupplierByRfcQueryHandler(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public async Task<SupplierDto?> Handle(
            GetSupplierByRfcQuery request,
            CancellationToken cancellationToken)
        {
            var supplier = await _repository.GetByRfcAsync(request.Rfc);

            if (supplier is null)
                return null;

            return new SupplierDto
            {
                IdSupplier = supplier.IdSupplier,
                Rfc = supplier.Rfc,
                BusinessName = supplier.BusinessName,
                Street = supplier.Street,
                ExternalNumber = supplier.ExternalNumber,
                InternalNumber = supplier.InternalNumber,
                Neighborhood = supplier.Neighborhood,
                PostalCode = supplier.PostalCode,
                City = supplier.City,
                Municipality = supplier.Municipality,
                State = supplier.State,
                Country = supplier.Country,
                Phone = supplier.Phone,
                ContactName = supplier.ContactName,
                ContactPhone = supplier.ContactPhone,
                Email = supplier.Email,
                Active = supplier.Active
            };
        }
    }
    }
