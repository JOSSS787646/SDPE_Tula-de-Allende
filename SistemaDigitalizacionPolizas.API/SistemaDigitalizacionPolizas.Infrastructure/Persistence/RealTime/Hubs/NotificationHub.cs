using Microsoft.AspNetCore.SignalR;


/// <summary>
/// Hub de SignalR para la comunicación en tiempo real de notificaciones.
///
/// Actúa como punto de conexión entre el servidor y los clientes,
/// permitiendo enviar eventos (como notificaciones) a usuarios conectados.
///
/// Es utilizado por servicios como SignalRNotificationService para
/// emitir mensajes hacia el frontend.
///
/// Nota:
/// No contiene lógica propia, ya que solo funciona como canal de comunicación.
/// </summary>
/// 

namespace SistemaDigitalizacionPolizas.API.SignalR
{
    public class NotificationHub : Hub
    {
    }
}
