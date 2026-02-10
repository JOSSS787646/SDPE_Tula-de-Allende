using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Community;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Commands.UpdateBeneficiary
{
    internal class UpdateBeneficiaryCommandHandler
        : IRequestHandler<UpdateBeneficiaryCommand, bool>
    {
        private readonly IBeneficiaryRepository _repository;
        private readonly ICurrentUserService _currentUserService;
        public UpdateBeneficiaryCommandHandler(IBeneficiaryRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(
       UpdateBeneficiaryCommand request,
       CancellationToken cancellationToken)
        {
            var dto = request.Beneficiary;

            // 1️⃣ Obtener entidad por el ID de la ruta (ID real)
            var entity = await _repository.GetByIdAsync(request.IdBeneficiary);

            if (entity is null)
                throw new KeyNotFoundException("Beneficiario no encontrado.");

            // 2️⃣ Regla de negocio:
            // El id del body puede venir en 0 (o null si así lo manejas),
            // pero NO puede ser diferente al id de la ruta si viene con valor.
            if (dto.IdBeneficiary != 0 && dto.IdBeneficiary != request.IdBeneficiary)
                throw new InvalidOperationException("No se permite modificar el ID del beneficiario.");

            // 3️⃣ (Opcional) Reglas de negocio para duplicados si cambian CURP/INE
            if (!string.Equals(entity.Curp, dto.Curp, StringComparison.OrdinalIgnoreCase))
            {
                var existsByCurp = await _repository.ExistsByCurpAsync(dto.Curp.Trim().ToUpper());
                if (existsByCurp)
                    throw new InvalidOperationException("Ya existe un beneficiario con esa CURP.");
            }

            if (!string.Equals(entity.Ine, dto.Ine, StringComparison.OrdinalIgnoreCase))
            {
                var existsByIne = await _repository.ExistsByIneAsync(dto.Ine.Trim().ToUpper());
                if (existsByIne)
                    throw new InvalidOperationException("Ya existe un beneficiario con ese INE.");
            }

            // 4️⃣ Mapear campos EDITABLES (NO tocar IdBeneficiary)
            entity.FirstName = dto.FirstName.Trim();
            entity.PaternalLastName = dto.PaternalLastName.Trim();
            entity.MaternalLastName = dto.MaternalLastName?.Trim();
            entity.Street = dto.Street.Trim();
            entity.ExternalNumber = dto.ExternalNumber;
            entity.InternalNumber = dto.InternalNumber;
            entity.Neighborhood = dto.Neighborhood;
            entity.PostalCode = dto.PostalCode;
            entity.City = dto.City;
            entity.Municipality = dto.Municipality;
            entity.State = dto.State;
            entity.Country = dto.Country;
            entity.Ine = dto.Ine.Trim().ToUpper();
            entity.Curp = dto.Curp.Trim().ToUpper();
            entity.Phone = dto.Phone;   // ahora string
            entity.Email = dto.Email;
            entity.Active = dto.Active;

            entity.ModifiedAt = DateTime.UtcNow;

            // 5️⃣ Persistir cambios
            return await _repository.UpdateAsync(entity);
        }


    }
}
