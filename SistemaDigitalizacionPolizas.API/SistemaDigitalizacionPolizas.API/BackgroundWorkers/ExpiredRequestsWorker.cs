using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.ProcessExpiredRequests;


/// <summary>
/// Worker en segundo plano que ejecuta diariamente el procesamiento
/// de solicitudes vencidas.
///
/// Crea un scope de dependencias para ejecutar el comando
/// ProcessExpiredRequestsCommand mediante MediatR, asegurando
/// aislamiento por ejecución.
///
/// Incluye control de tiempo (timeout de 5 minutos), manejo de errores
/// y ejecución periódica cada 24 horas.
///
/// Su objetivo es automatizar la actualización de solicitudes expiradas
/// sin intervención manual.
/// </summary>
/// 


namespace SistemaDigitalizacionPolizas.API.BackgroundWorkers
{
    public class ExpiredRequestsWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ExpiredRequestsWorker> _logger;

        public ExpiredRequestsWorker(
            IServiceScopeFactory scopeFactory,
            ILogger<ExpiredRequestsWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("[ExpiredRequestsWorker] Worker iniciado: {Time}", DateTimeOffset.UtcNow);

            // Espera 30s al arrancar para que la app termine de inicializarse
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("[ExpiredRequestsWorker] Iniciando ciclo: {Time}", DateTimeOffset.UtcNow);

                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    // Timeout de 5 minutos por si el proceso se cuelga
                    using var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                    cts.CancelAfter(TimeSpan.FromMinutes(5));

                    await mediator.Send(new ProcessExpiredRequestsCommand(), cts.Token);

                    _logger.LogInformation("[ExpiredRequestsWorker] Ciclo completado exitosamente: {Time}", DateTimeOffset.UtcNow);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    // Apagado limpio de la app, no es error
                    _logger.LogInformation("[ExpiredRequestsWorker] Worker detenido por señal de apagado.");
                    break;
                }
                catch (OperationCanceledException)
                {
                    // Timeout de 5 minutos
                    _logger.LogError("[ExpiredRequestsWorker] El proceso excedió el tiempo límite de 5 minutos.");
                }
                catch (Exception ex)
                {
                    // Error inesperado — loguea pero NO detiene el worker
                    _logger.LogError(ex, "[ExpiredRequestsWorker] Error en el ciclo. Se reintentará en el próximo intervalo.");
                }
                finally
                {
                    _logger.LogInformation("[ExpiredRequestsWorker] Próxima ejecución en 24 horas: {Time}",
                        DateTimeOffset.UtcNow.AddHours(24));
                }

                try
                {
                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // App se está apagando durante el Delay, salir limpio
                    break;
                }
            }

            _logger.LogInformation("[ExpiredRequestsWorker] Worker finalizado: {Time}", DateTimeOffset.UtcNow);
        }
    }
}