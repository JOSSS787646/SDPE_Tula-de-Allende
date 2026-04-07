using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service.Gmail_Services;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.ProcessExpiredRequests
{
    public class ProcessExpiredRequestsHandler : IRequestHandler<ProcessExpiredRequestsCommand>
    {
        private readonly IAcquisitionRequest _repository;
        private readonly EmailQueue _queue;
        private readonly IEmailService _emailService;

        public ProcessExpiredRequestsHandler(
            IAcquisitionRequest repository,
            EmailQueue queue,
            IEmailService emailService)
        {
            _repository = repository;
            _queue = queue;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(ProcessExpiredRequestsCommand request, CancellationToken cancellationToken)
        {
            var expiredRequests = await _repository.GetExpiredRequestsAsync();

            if (!expiredRequests.Any())
                return Unit.Value;

            // 🔥 AGRUPAR POR USUARIO (CLAVE)
            var groupedByUser = expiredRequests
                .GroupBy(x => new { x.UserEmail, x.UserName })
                .ToList();

            foreach (var group in groupedByUser)
            {
                var email = group.Key.UserEmail;
                var name = group.Key.UserName;
                var requests = group.ToList();

                // 🔥 ENCOLAR (NO BLOQUEAR)
                _queue.Enqueue(async () =>
                {
                    await _emailService.SendGroupedExpiredNotificationAsync(
                        email,
                        name,
                        requests
                    );
                });

                // 🔥 ACTUALIZAR CONTROL ANTI-SPAM
                foreach (var req in requests)
                {
                    await _repository.UpdateNotificationMetadataAsync(req.RequestId);
                }
            }

            return Unit.Value;
        }
    }
}
