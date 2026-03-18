using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.CreateNotification;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.MarkAllNotificationsAsRead;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.MarkNotificationAsReadC;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Queries.GetAllNotificationsByUser;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Queries.GetUnreadCount;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Queries.GetUnreadNotificationsByUser;
using SistemaDigitalizacionPolizas.Domain.Dtos.Notificacion;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    //[Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 🔔 Obtener notificaciones NO leídas
        [HttpGet("unread/{userId}")]
        public async Task<ActionResult<List<NotificationDto>>> GetUnreadByUser(int userId)
        {
            var result = await _mediator.Send(
                new GetUnreadNotificationsByUserQuery(userId)
            );

            // 🔥 puedes decidir cómo responder
            if (result == null || result.Count == 0)
                return Ok(new List<NotificationDto>()); // mejor que NoContent

            return Ok(result);
        }


        // 📚 Historial de notificaciones (todas)
        [HttpGet("all/{userId}")]
        public async Task<ActionResult<List<NotificationDto>>> GetAllByUser(int userId)
        {
            var result = await _mediator.Send(
                new GetAllNotificationsByUserQuery(userId)
            );

            return Ok(result); // siempre lista (aunque esté vacía)
        }


        // ✔ Marcar como leída
        [HttpPut("read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _mediator.Send(new MarkNotificationAsReadCommand(id));

            return NoContent(); // 204 OK
        }

        // ✔ Marcar TODAS como leídas
        [HttpPut("read-all/{userId}")]
        public async Task<IActionResult> MarkAllAsRead(int userId)
        {
            await _mediator.Send(new MarkAllNotificationsAsReadCommand(userId));

            return NoContent();
        }

        // 🔔 Contador de no leídas
        [HttpGet("unread/count/{userId}")]
        public async Task<ActionResult<int>> GetUnreadCount(int userId)
        {
            var count = await _mediator.Send(new GetUnreadCountQuery(userId));

            return Ok(count);
        }


        // ➕ Crear notificación
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateNotificationCommand command)
        {
            var id = await _mediator.Send(command);

            return Ok(id); 
        }

    }
}
