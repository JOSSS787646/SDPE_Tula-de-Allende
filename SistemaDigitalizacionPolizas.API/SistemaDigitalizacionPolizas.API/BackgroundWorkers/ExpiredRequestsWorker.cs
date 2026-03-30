using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.ProcessExpiredRequests;

namespace SistemaDigitalizacionPolizas.API.BackgroundWorkers
{
    public class ExpiredRequestsWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ExpiredRequestsWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Send(new ProcessExpiredRequestsCommand());

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
