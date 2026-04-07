using Microsoft.AspNetCore.SignalR;
using SistemaDigitalizacionPolizas.API.SignalR;
using SistemaDigitalizacionPolizas.Application.Interfaces;

using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;

/// <summary>
/// Implementación del servicio de notificaciones en tiempo real utilizando SignalR.
///
/// Se encarga de enviar notificaciones instantáneas a usuarios específicos
/// conectados al sistema mediante WebSockets.
///
/// Responsabilidad:
/// - Enviar eventos de notificación al cliente correspondiente.
/// - Mapear la entidad Notification a un objeto ligero para el cliente.
/// - Utilizar SignalR Hub para comunicación en tiempo real.
///
/// Uso:
/// Este servicio es invocado desde la capa de aplicación cuando ocurre
/// un evento relevante (ej. carga de documentos, revisión, cambios de estado).
///
/// Funcionamiento:
/// - Identifica al usuario destino mediante su UserId.
/// - Envía la notificación al cliente conectado con ese identificador.
/// - Dispara el evento "ReceiveNotification" en el frontend.
///
/// Nota:
/// Requiere que el cliente esté autenticado y conectado al NotificationHub
/// para recibir las notificaciones.
/// </summary>
public class SignalRNotificationService : IRealtimeNotificationService
{
    private readonly IHubContext<NotificationHub> _hub;

    public SignalRNotificationService(
        IHubContext<NotificationHub> hub)
    {
        _hub = hub;
    }

    /// <summary>
    /// Envía una notificación en tiempo real al usuario especificado.
    /// </summary>
    public async Task SendAsync(Notification notification)
    {
        await _hub.Clients
            .User(notification.UserId.ToString())
            .SendAsync("ReceiveNotification", new
            {
                id = notification.IdNotification,
                title = notification.Title,
                message = notification.Message,
                requestId = notification.RequestId,
                createdAt = notification.CreatedAt
            });
    }
}