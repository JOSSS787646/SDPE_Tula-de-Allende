using SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestNotification
{

    /// <summary>
    /// Interfaz que define el repositorio para obtener información necesaria
    /// para el envío de notificaciones relacionadas a solicitudes y documentos.
    ///
    /// Proporciona datos específicos para distintos tipos de notificación
    /// como carga, aprobación y observación de documentos.
    /// </summary>
    public interface IRequestNotificationRepository
    {
        /// <summary>
        /// Obtiene la información general de una solicitud necesaria
        /// para generar notificaciones.
        /// </summary>
        Task<RequestNotificationDto?> GetRequestNotificationInfoAsync(int requestId);

        /// <summary>
        /// Obtiene la información necesaria para notificar que un documento
        /// ha sido aprobado dentro de una solicitud.
        /// </summary>
        Task<DocumentApprovedNotificationDto?> GetDocumentApprovedInfoAsync(int requestId);

        /// <summary>
        /// Obtiene la información necesaria para notificar que un documento
        /// ha sido observado dentro de una solicitud.
        /// </summary>
        Task<DocumentObservedNotificationDto?> GetDocumentObservedInfoAsync(int requestId);
    }
}
