using SistemaDigitalizacionPolizas.Domain.Dtos.ApplicationStatus;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SSistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AppliccationStatus_Service.Queries.GetApplicationStatusByCode
{
    public class GetApplicationStatusByCodeQueryHandler
        : IRequestHandler<GetApplicationStatusByCodeQuery, ApplicationStatusDto?>
    {
        private readonly IApplicationStatusRepository _repository;

        public GetApplicationStatusByCodeQueryHandler(
            IApplicationStatusRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApplicationStatusDto?> Handle(
            GetApplicationStatusByCodeQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByCodeAsync(request.Code);

            if (entity == null)
                return null;

            return new ApplicationStatusDto
            {
                IdApplicationStatus = entity.IdApplicationStatus,
                Code = entity.Code,
                Description = entity.Description,
                Order = entity.Order,
                Active = entity.Active
            };
        }
    }
}
