using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Queries.GetByCodeFundingSource;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Queries.GetProyectByCode
{
    public class GetProyectByCodeQueryHandler
        : IRequestHandler<GetProyectByCodeQuery, ProyectDto>
    {
        private readonly IProyectRepository _repository;
        public  GetProyectByCodeQueryHandler(IProyectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProyectDto?> Handle(
            GetProyectByCodeQuery request,
            CancellationToken cancellationToken)
        {
            var proyect = await _repository.GetByCodeAsync(request.code);
            if (proyect == null)
            {
                return null;
            }
            return new ProyectDto
            {
                idProyect = proyect.idProyect,
                Code = proyect.Code,
                Description = proyect.Description,
                Active = proyect.Active
            };
        }
    }
}
