using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service.Gmail_Services;

namespace SistemaDigitalizacionPolizas.API.BackgroundWorkers
{


    namespace SistemaDigitalizacionPolizas.API.BackgroundWorkers
    {
        public class EmailBackgroundWorker : BackgroundService
        {
            private readonly EmailQueue _queue;

            private const int MaxRetries = 3;

            public EmailBackgroundWorker(EmailQueue queue)
            {
                _queue = queue;
            }

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        if (_queue.TryDequeue(out var emailTask))
                        {
                            var attempt = 0;
                            var sent = false;

                            while (attempt < MaxRetries && !sent)
                            {
                                try
                                {
                                    attempt++;

                                    await emailTask();

                                    sent = true;
                                }
                                catch
                                {
                                    if (attempt >= MaxRetries)
                                        break;

                                    var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt));
                                    await Task.Delay(delay, stoppingToken);
                                }
                            }
                        }

                        await Task.Delay(300, stoppingToken);
                    }
                    catch
                    {
                        await Task.Delay(2000, stoppingToken);
                    }
                }
            }
        }
    }
}
