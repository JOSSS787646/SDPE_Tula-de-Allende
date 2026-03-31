using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service.Gmail_Services;

namespace SistemaDigitalizacionPolizas.API.BackgroundWorkers
{
    public class EmailBackgroundWorker : BackgroundService
    {
        private readonly EmailQueue _queue;
        private readonly ILogger<EmailBackgroundWorker> _logger;
        private const int MaxRetries = 3;

        public EmailBackgroundWorker(EmailQueue queue, ILogger<EmailBackgroundWorker> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("[EmailWorker] Worker de correo iniciado: {Time}", DateTimeOffset.UtcNow);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_queue.TryDequeue(out var emailTask))
                    {
                        _logger.LogInformation("[EmailWorker] Tarea de correo desencolada, iniciando envío.");

                        var attempt = 0;
                        var sent = false;

                        while (attempt < MaxRetries && !sent)
                        {
                            try
                            {
                                attempt++;
                                _logger.LogInformation("[EmailWorker] Intento {Attempt}/{Max}", attempt, MaxRetries);

                                await emailTask();

                                sent = true;
                                _logger.LogInformation("[EmailWorker] Correo enviado exitosamente en intento {Attempt}.", attempt);
                            }
                            catch (OperationCanceledException)
                            {
                                // App apagándose, salir limpio
                                _logger.LogWarning("[EmailWorker] Envío cancelado por apagado de la app.");
                                return;
                            }
                            catch (Exception ex)
                            {
                                if (attempt >= MaxRetries)
                                {
                                    _logger.LogError(ex,
                                        "[EmailWorker] Falló el envío después de {Max} intentos. Tarea descartada.",
                                        MaxRetries);
                                    break;
                                }

                                var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt));
                                _logger.LogWarning(ex,
                                    "[EmailWorker] Intento {Attempt} fallido. Reintentando en {Delay}s.",
                                    attempt, delay.TotalSeconds);

                                await Task.Delay(delay, stoppingToken);
                            }
                        }
                    }

                    await Task.Delay(300, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("[EmailWorker] Worker detenido por señal de apagado.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[EmailWorker] Error inesperado en el loop principal. Reintentando en 2s.");
                    await Task.Delay(2000, stoppingToken);
                }
            }

            _logger.LogInformation("[EmailWorker] Worker de correo finalizado: {Time}", DateTimeOffset.UtcNow);
        }
    }
}