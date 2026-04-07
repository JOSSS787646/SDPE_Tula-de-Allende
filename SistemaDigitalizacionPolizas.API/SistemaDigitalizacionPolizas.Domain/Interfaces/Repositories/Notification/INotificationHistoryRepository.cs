using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IANotification
{
    /// <summary>
    /// Interfaz que define el repositorio para el registro del historial
    /// de notificaciones enviadas.
    ///
    /// Permite almacenar de forma individual o masiva los eventos de notificación,
    /// facilitando auditoría y trazabilidad.
    /// </summary>
    public interface INotificationHistoryRepository
    {
        /// <summary>
        /// Registra una notificación en el historial.
        /// </summary>
        Task AddAsync(NotificationHistory entity);

        /// <summary>
        /// Registra múltiples notificaciones en una sola operación.
        /// </summary>
        Task AddRangeAsync(IEnumerable<NotificationHistory> entities);
    }
}
