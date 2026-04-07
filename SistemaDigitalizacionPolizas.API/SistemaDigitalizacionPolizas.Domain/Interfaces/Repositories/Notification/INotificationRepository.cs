using SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion;
using SistemaDigitalizacionPolizas.Domain.Entities.Notification_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification
{
    /// <summary>
    /// Interfaz que define el repositorio para la gestión de notificaciones.
    ///
    /// Permite consultar, crear, actualizar y eliminar notificaciones,
    /// así como gestionar su estado de lectura y obtener información
    /// relacionada para visualización.
    /// </summary>
    public interface INotificationRepository
    {
        /// <summary>
        /// Obtiene las notificaciones no leídas de un usuario.
        /// </summary>
        Task<List<Notification>> GetUnreadByUserAsync(int userId);

        /// <summary>
        /// Obtiene todas las notificaciones (historial) de un usuario.
        /// </summary>
        Task<List<Notification>> GetAllByUserAsync(int userId);

        /// <summary>
        /// Obtiene una notificación por su identificador.
        /// </summary>
        Task<Notification?> GetByIdAsync(int notificationId);

        /// <summary>
        /// Obtiene el detalle de una notificación y la marca como leída.
        /// </summary>
        Task<Notification?> GetAndMarkAsReadAsync(int notificationId);

        /// <summary>
        /// Crea una nueva notificación.
        /// </summary>
        Task AddAsync(Notification notification);

        /// <summary>
        /// Marca una notificación como leída.
        /// </summary>
        Task MarkAsReadAsync(int notificationId);

        /// <summary>
        /// Marca todas las notificaciones de un usuario como leídas.
        /// </summary>
        Task MarkAllAsReadAsync(int userId);

        /// <summary>
        /// Obtiene la cantidad de notificaciones no leídas de un usuario.
        /// </summary>
        Task<int> GetUnreadCountAsync(int userId);

        /// <summary>
        /// Obtiene información de vista previa de documentos cargados
        /// asociados a una solicitud.
        /// </summary>
        Task<DocumentPreviewInfoDto?> GetDocumentsUploadedPreviewAsync(int requestId);

        /// <summary>
        /// Elimina una notificación por su identificador.
        /// </summary>
        Task<bool> DeleteAsync(int notificationId);

        /// <summary>
        /// Elimina todas las notificaciones de un usuario.
        /// </summary>
        Task DeleteAllByUserAsync(int userId);
    }
}
