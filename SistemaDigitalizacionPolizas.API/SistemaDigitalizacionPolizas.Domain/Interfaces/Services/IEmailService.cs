using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;

/// <summary>
/// Interfaz que define el servicio de envío de correos electrónicos del sistema.
/// Maneja notificaciones relacionadas al flujo de solicitudes y documentos.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Envía un correo genérico.
    /// 
    /// Se usa para notificaciones simples donde solo se requiere
    /// destinatario, asunto y contenido.
    /// </summary>
    Task SendAsync(string to, string subject, string body);

    /// <summary>
    /// Notifica al usuario que se han cargado documentos en una solicitud.
    /// 
    /// Incluye información de la solicitud como número, unidad administrativa,
    /// descripción, fecha y lista de documentos cargados.
    /// </summary>
    Task SendDocumentsUploadedAsync(
        string to,
        string userName,
        string requestId,
        string administrativeUnit,
        string requestDescription,
        DateTime date,
        string documentsList);

    /// <summary>
    /// Notifica el resultado de la revisión de un documento específico.
    /// 
    /// Informa si fue aprobado o rechazado, incluyendo observaciones
    /// en caso de existir.
    /// </summary>
    Task SendDocumentReviewedAsync(
        string toEmail,
        string reviewerName,
        string requestNumber,
        string documentName,
        string result,
        string? observations
    );

    /// <summary>
    /// Envía una notificación de revisión de documento (puede ser usada
    /// para alertar a otros involucrados además del usuario principal).
    /// 
    /// Contiene la misma información que el resultado de revisión.
    /// </summary>
    Task SendDocumentReviewNotificationAsync(
        string toEmail,
        string reviewerName,
        string requestNumber,
        string documentName,
        string result,
        string? observations
    );

    /// <summary>
    /// Envía una notificación agrupada de solicitudes vencidas.
    /// 
    /// Permite notificar en un solo correo múltiples solicitudes expiradas,
    /// evitando envíos masivos individuales (spam).
    /// </summary>
    Task SendGroupedExpiredNotificationAsync(
        string to,
        string userName,
        List<ExpiredRequestDto> requests);
}