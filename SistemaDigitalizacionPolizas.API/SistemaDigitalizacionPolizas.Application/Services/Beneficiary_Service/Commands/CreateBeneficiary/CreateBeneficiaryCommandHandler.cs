using MediatR;
using SistemaDigitalizacionPolizas.Domain.Dtos.Community;
using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Community;

namespace SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Commands.CreateBeneficiary
{
    internal class CreateBeneficiaryCommandHandler
        : IRequestHandler<CreateBeneficiaryCommand, int>
    {
        private readonly IBeneficiaryRepository _repository;

        public CreateBeneficiaryCommandHandler(IBeneficiaryRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(
            CreateBeneficiaryCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Beneficiary;

       
            var existsByCurp = await _repository.ExistsByCurpAsync(dto.Curp);
            if (existsByCurp)
                throw new InvalidOperationException("Ya existe un beneficiario con esa CURP.");

            // 2️⃣ Validar INE única
            var existsByIne = await _repository.ExistsByIneAsync(dto.Ine);
            if (existsByIne)
                throw new InvalidOperationException("Ya existe un beneficiario con ese INE.");

            // 3️⃣ Mapear DTO → Entidad
            var entity = new Beneficiary
            {
                IdBeneficiary = dto.IdBeneficiary,  
                FirstName = dto.FirstName.Trim(),
                PaternalLastName = dto.PaternalLastName.Trim(),
                MaternalLastName = dto.MaternalLastName?.Trim(),
                Street = dto.Street.Trim(),
                ExternalNumber = dto.ExternalNumber,
                InternalNumber = dto.InternalNumber,
                Neighborhood = dto.Neighborhood,
                PostalCode = dto.PostalCode,
                City = dto.City,
                Municipality = dto.Municipality,
                State = dto.State,
                Country = dto.Country,
                Ine = dto.Ine.Trim().ToUpper(),
                Curp = dto.Curp.Trim().ToUpper(),
                Phone = dto.Phone,
                Email = dto.Email,
                Active = dto.Active
            };

            var beneficiary = await _repository.AddAsync(entity);

            if (beneficiary is null)
                throw new InvalidOperationException("No se pudo crear el beneficiario (CURP duplicada).");

            return beneficiary.IdBeneficiary;

        }
    }
}
