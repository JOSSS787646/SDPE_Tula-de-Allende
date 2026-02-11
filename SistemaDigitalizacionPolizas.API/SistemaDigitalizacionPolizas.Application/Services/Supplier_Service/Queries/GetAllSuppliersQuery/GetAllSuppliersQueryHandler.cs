using SistemaDigitalizacionPolizas.Domain.Dtos.Supplier;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Supplier_Service.Queries.GetAllSuppliersQuery
{
    public class GetAllSuppliersQueryHandler
        : IRequestHandler<GetAllSuppliersQuery, List<SupplierDto>>
    {

        private readonly ISupplierRepository _repository;

        public GetAllSuppliersQueryHandler(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SupplierDto>> Handle(
            GetAllSuppliersQuery request,
            CancellationToken cancellationToken)
        {
            int pageSize = 50;
            int page = request.Page <= 0 ? 1 : request.Page;

            var suppliers = await _repository.GetPagedAsync(page, pageSize);

            return suppliers.Select(s => new SupplierDto
            {
                IdSupplier = s.IdSupplier,
                Rfc = s.Rfc,
                BusinessName = s.BusinessName,
                Street = s.Street,
                ExternalNumber = s.ExternalNumber,
                InternalNumber = s.InternalNumber,
                Neighborhood = s.Neighborhood,
                PostalCode = s.PostalCode,
                City = s.City,
                Municipality = s.Municipality,
                State = s.State,
                Country = s.Country,
                Phone = s.Phone,
                ContactName = s.ContactName,
                ContactPhone = s.ContactPhone,
                Email = s.Email,
                Active=s.Active
            }).ToList();
        }
    }
}
