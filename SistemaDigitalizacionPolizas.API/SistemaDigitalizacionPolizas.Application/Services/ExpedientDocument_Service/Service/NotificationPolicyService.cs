using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.ISystemConfiguration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service
{
    public class NotificationPolicyService : INotificationPolicyService
    {
        private readonly ISystemConfigurationRepository _configurationRepository;
        private readonly IRequestNotificationRepository _requestRepository;

        public NotificationPolicyService(
            ISystemConfigurationRepository configurationRepository,
            IRequestNotificationRepository requestRepository)
        {
            _configurationRepository = configurationRepository;
            _requestRepository = requestRepository;
        }

        public async Task<bool> ShouldSendNotificationAsync(int requestId)
        {
            var config = await _configurationRepository.GetActiveAsync();

            if (config == null || !config.EmailsEnabled)
                return false;

            if (!config.NotificationStartDate.HasValue)
                return true;

            var requestInfo = await _requestRepository
                .GetRequestNotificationInfoAsync(requestId);

            if (requestInfo == null)
                return false;

            return requestInfo.RequestDate >= config.NotificationStartDate.Value;
        }
    }
}
