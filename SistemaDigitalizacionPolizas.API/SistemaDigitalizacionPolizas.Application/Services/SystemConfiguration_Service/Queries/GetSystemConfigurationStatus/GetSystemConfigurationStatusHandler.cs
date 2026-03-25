using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.ISystemConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.SystemConfiguration_Service.Queries.GetSystemConfigurationStatus
{
    public class GetSystemConfigurationStatusHandler
      : IRequestHandler<GetSystemConfigurationStatusCommand, GetSystemConfigurationStatusResponse>
    {
        private readonly ISystemConfigurationRepository _repository;

        public GetSystemConfigurationStatusHandler(ISystemConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetSystemConfigurationStatusResponse> Handle(
            GetSystemConfigurationStatusCommand request,
            CancellationToken cancellationToken)
        {
            var config = await _repository.GetActiveAsync();

            if (config == null)
            {
                return new GetSystemConfigurationStatusResponse(
                    false,
                    string.Empty
                );
            }

            return new GetSystemConfigurationStatusResponse(
             config.EmailsEnabled,
             config.NotificationStartDate?.ToString("yyyy-MM-dd")
            );
        }
    }
}
