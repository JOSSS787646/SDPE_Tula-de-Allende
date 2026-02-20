using SistemaDigitalizacionPolizas.Domain.Dtos.Beneficiary;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Queries.GetBeneficiaryByCurp
{
    public class GetBeneficiaryByCurpQueryHandler
        : IRequestHandler<GetBeneficiaryByCurpQuery, BeneficiaryDto?>
    {
        private readonly IBeneficiaryRepository _repository;

        public GetBeneficiaryByCurpQueryHandler(IBeneficiaryRepository repository)
        {
            _repository = repository;
        }

        public async Task<BeneficiaryDto?> Handle(
            GetBeneficiaryByCurpQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByCurpAsync(request.Curp);

            if (entity is null)
                return null;

            return new BeneficiaryDto
            {
                IdBeneficiary = entity.IdBeneficiary,
                FirstName = entity.FirstName,
                PaternalLastName = entity.PaternalLastName,
                MaternalLastName = entity.MaternalLastName,
                Street = entity.Street,
                ExternalNumber = entity.ExternalNumber,
                InternalNumber = entity.InternalNumber,
                Neighborhood = entity.Neighborhood,
                PostalCode = entity.PostalCode,
                City = entity.City,
                Municipality = entity.Municipality,
                State = entity.State,
                Country = entity.Country,
                Ine = entity.Ine,
                Curp = entity.Curp,
                Phone = entity.Phone,
                Email = entity.Email,
                Active = entity.Active
            };
        }
    }
}
