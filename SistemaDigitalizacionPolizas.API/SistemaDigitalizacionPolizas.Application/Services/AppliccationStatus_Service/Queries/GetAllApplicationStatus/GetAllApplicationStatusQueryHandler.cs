using SistemaDigitalizacionPolizas.Domain.Dtos.ApplicationStatus;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SSistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AppliccationStatus_Service.Queries.GetAllApplicationStatus
{
    public class GetAllApplicationStatusQueryHandler
        : IRequestHandler<GetAllApplicationStatusQuery, List<ApplicationStatusDto>>
    {
        private readonly IApplicationStatusRepository _repository;

        public GetAllApplicationStatusQueryHandler(
            IApplicationStatusRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ApplicationStatusDto>> Handle(
            GetAllApplicationStatusQuery request,
            CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync();

            return entities
                .OrderBy(x => x.Order) // 🔥 importante para flujo visual
                .Select(entity => new ApplicationStatusDto
                {
                    IdApplicationStatus = entity.IdApplicationStatus,
                    Code = entity.Code,
                    Description = entity.Description,
                    Order = entity.Order,
                    Active = entity.Active
                })
                .ToList();
        }
    }
}
