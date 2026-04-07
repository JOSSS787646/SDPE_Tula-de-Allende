using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.CreateNotification;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.DeleteAllNotificationsByUser;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.DeleteNotification;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.MarkAllNotificationsAsRead;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.MarkNotificationAsReadC;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Queries.GetAllNotificationsByUser;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Queries.GetUnreadCount;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Queries.GetUnreadNotificationsByUser;
using SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Gestiona notificaciones (consulta, creación y actualización de lectura).
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtener notificaciones no leídas.
        /// </summary>
        [HttpGet("unread/{userId}")]
        public async Task<ActionResult<List<NotificationDto>>> GetUnread(int userId)
        {
            var result = await _mediator.Send(
                new GetUnreadNotificationsByUserQuery(userId)
            );

            return Ok(result ?? new List<NotificationDto>());
        }

        /// <summary>
        /// Obtener todas las notificaciones.
        /// </summary>
        [HttpGet("all/{userId}")]
        public async Task<ActionResult<List<NotificationDto>>> GetAll(int userId)
        {
            var result = await _mediator.Send(
                new GetAllNotificationsByUserQuery(userId)
            );

            return Ok(result);
        }

        /// <summary>
        /// Marcar notificación como leída.
        /// </summary>
        [HttpPut("{id:int}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _mediator.Send(new MarkNotificationAsReadCommand(id));

            return NoContent();
        }

        /// <summary>
        /// Marcar todas como leídas.
        /// </summary>
        [HttpPut("read-all/{userId}")]
        public async Task<IActionResult> MarkAllAsRead(int userId)
        {
            await _mediator.Send(new MarkAllNotificationsAsReadCommand(userId));

            return NoContent();
        }

        /// <summary>
        /// Obtener cantidad de no leídas.
        /// </summary>
        [HttpGet("unread/count/{userId}")]
        public async Task<ActionResult<int>> GetUnreadCount(int userId)
        {
            var count = await _mediator.Send(new GetUnreadCountQuery(userId));

            return Ok(count);
        }

        /// <summary>
        /// Crear notificación.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateNotificationCommand command)
        {
            var id = await _mediator.Send(command);

            return Ok(id);
        }

        /// <summary>
        /// Eliminar notificación.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteNotificationCommand(id));

            return NoContent();
        }

        /// <summary>
        /// Eliminar todas las notificaciones de un usuario.
        /// </summary>
        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteAllByUser(int userId)
        {
            await _mediator.Send(new DeleteAllNotificationsByUserCommand(userId));

            return NoContent();
        }
    }
}