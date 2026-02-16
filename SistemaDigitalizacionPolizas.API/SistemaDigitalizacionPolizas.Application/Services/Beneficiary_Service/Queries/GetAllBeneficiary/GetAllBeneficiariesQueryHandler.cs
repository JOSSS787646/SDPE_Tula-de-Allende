using SistemaDigitalizacionPolizas.Domain.Dtos.Community;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Beneficiary_Service.Queries.GetAllBeneficiary
{
    public class GetAllBeneficiariesQueryHandler
        : IRequestHandler<GetAllBeneficiariesQuery, List<BeneficiaryDto>>
    {
        private readonly IBeneficiaryRepository _repository;

        public GetAllBeneficiariesQueryHandler(IBeneficiaryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<BeneficiaryDto>> Handle(
            GetAllBeneficiariesQuery request,
            CancellationToken cancellationToken)
        {
            var data = await _repository.GetAllAsync();

            // Mapeo Entidad → DTO
            return data.Select(b => new BeneficiaryDto
            {
                IdBeneficiary = b.IdBeneficiary,
                FirstName = b.FirstName,
                PaternalLastName = b.PaternalLastName,
                MaternalLastName = b.MaternalLastName,
                Street = b.Street,
                ExternalNumber = b.ExternalNumber,
                InternalNumber = b.InternalNumber,
                Neighborhood = b.Neighborhood,
                PostalCode = b.PostalCode,
                City = b.City,
                Municipality = b.Municipality,
                State = b.State,
                Country = b.Country,
                Ine = b.Ine,
                Curp = b.Curp,
                Phone = b.Phone,
                Email = b.Email,
                Active = b.Active
            }).ToList();
        }
    }
}
