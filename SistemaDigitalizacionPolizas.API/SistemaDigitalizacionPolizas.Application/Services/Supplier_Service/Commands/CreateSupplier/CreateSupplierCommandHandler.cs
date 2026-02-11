using MediatR;
using SistemaDigitalizacionPolizas.Domain.Entities.Supplier_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Suppliers;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

namespace SistemaDigitalizacionPolizas.Application.Services.Supplier_Service.Commands.CreateSupplier
{
    public class CreateSupplierCommandHandler
        : IRequestHandler<CreateSupplierCommand, int>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateSupplierCommandHandler(
            ISupplierRepository supplierRepository,
            ICurrentUserService currentUserService)
        {
            _supplierRepository = supplierRepository;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(
            CreateSupplierCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Supplier;

            // 1) Regla de negocio: RFC único
            var exists = await _supplierRepository.ExistsByRfcAsync(dto.Rfc);
            if (exists)
                throw new InvalidOperationException($"Ya existe un proveedor con el RFC {dto.Rfc}.");

            // Teléfono único (opcional)
            if (!string.IsNullOrWhiteSpace(dto.Phone))
            {
                var existsByPhone = await _supplierRepository.ExistsByPhoneAsync(dto.Phone.Trim());
                if (existsByPhone)
                    throw new InvalidOperationException($"Ya existe un proveedor con el teléfono {dto.Phone}.");
            }
            // 2) Mapear DTO → Entidad
            var entity = new Supplier
            {
                Rfc = dto.Rfc.Trim().ToUpper(),
                BusinessName = dto.BusinessName.Trim(),

                Street = dto.Street.Trim(),
                ExternalNumber = dto.ExternalNumber,
                InternalNumber = dto.InternalNumber,
                Neighborhood = dto.Neighborhood,
                PostalCode = dto.PostalCode,

                City = dto.City,
                Municipality = dto.Municipality,
                State = dto.State,
                Country = dto.Country,

                Phone = dto.Phone,
                ContactName = dto.ContactName,
                ContactPhone = dto.ContactPhone,
                Email = dto.Email,

                Active = true,
                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTime.UtcNow
            };

            // 3) Persistir
            var created = await _supplierRepository.AddAsync(entity);

            // 4) Retornar Id creado
            return created.IdSupplier;
        }
    }
}
