using SistemaDigitalizacionPolizas.Domain.Entities.SystemConfiguration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.ISystemConfiguration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.SystemConfiguration_Service.Commands.UpsertSystemConfiguration
{
    public class UpsertSystemConfigurationHandler
            : IRequestHandler<UpsertSystemConfigurationCommand, int>
    {
        private readonly ISystemConfigurationRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public UpsertSystemConfigurationHandler(
            ISystemConfigurationRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(UpsertSystemConfigurationCommand request, CancellationToken cancellationToken)
        {
            var configuration = await _repository.GetActiveAsync();

            if (configuration == null)
            {
                configuration = new SystemConfiguration
                {
                    EmailsEnabled = request.EmailsEnabled,
                    NotificationStartDate = request.EmailsEnabled ? request.NotificationStartDate : null,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = _currentUserService.UserId,
                    Active = true
                };

                await _repository.AddAsync(configuration);
            }
            else
            {
                configuration.EmailsEnabled = request.EmailsEnabled;

                configuration.NotificationStartDate = request.EmailsEnabled
                    ? request.NotificationStartDate
                    : null;

                await _repository.UpdateAsync(configuration);
            }

            return configuration.IdConfiguration;
        }
    }
}
