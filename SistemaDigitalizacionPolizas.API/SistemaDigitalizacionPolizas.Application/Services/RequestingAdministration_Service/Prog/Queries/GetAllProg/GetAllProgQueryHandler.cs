using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Queries.CreateQuery;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Queries.GetAllProyect;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Queries.GetAllProg
{
    public class GetAllProgQueryHandler
    : IRequestHandler<GetAllProgQuery, List<ProgDto>>
    {
        private readonly IProgRepository _repository;

        public GetAllProgQueryHandler(IProgRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProgDto>> Handle(
     GetAllProgQuery request,
     CancellationToken cancellationToken)
        {
            var prog = await _repository.GetAllAsync();

            return prog.Select(r => new ProgDto
            {
                idProg = r.idProg,
                Code = r.Code,
                Description = r.Description,
                Active = r.Active
            }).ToList();
        }

    }

}
