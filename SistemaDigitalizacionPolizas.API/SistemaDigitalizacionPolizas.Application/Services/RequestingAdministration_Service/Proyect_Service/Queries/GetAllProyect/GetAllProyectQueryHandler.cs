using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.FundingSource_Service.Queries.GetAllFundingSource;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Queries.GetAllProyect
{
    public class GetAllProyectQueryHandler
        : IRequestHandler<GetAllProyectQuery, List<ProyectDto>>
    {

        private readonly IProyectRepository _repository;
        public GetAllProyectQueryHandler(IProyectRepository repository)
        {
            _repository = repository;
        }


        public async Task<List<ProyectDto>> Handle(
        GetAllProyectQuery request,
        CancellationToken cancellationToken)
        {
            var proyects = await _repository.GetAllAsync();

            return proyects.Select(r => new ProyectDto
            {
                idProyect=r.idProyect,
                Code = r.Code,
                Description = r.Description,
                Active = r.Active
            }).ToList();
        }



    }
}
