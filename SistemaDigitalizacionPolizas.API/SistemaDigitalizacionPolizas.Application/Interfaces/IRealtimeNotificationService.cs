using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Interfaces
{

    /// <summary>
    /// Interfaz que define el servicio de notificaciones en tiempo real.
    ///
    /// Permite enviar notificaciones instantáneas a los usuarios
    /// (ej. mediante WebSockets o SignalR), sin necesidad de recargar la aplicación.
    ///
    /// Se utiliza para informar eventos como cambios de estado, revisiones
    /// o acciones relevantes dentro del sistema.
    /// </summary>
    /// 

    public interface IRealtimeNotificationService
    {
        Task SendAsync(Notification notification);
    }
}
