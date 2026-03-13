using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.MarkAllNotificationsAsRead;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.MarkNotificationAsRead;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Queries.GetNotificationsByUser;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Queries.GetUnreadCount;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetNotifications(int userId)
        {
            var result = await _mediator.Send(
                new GetNotificationsByUserQuery(userId));

            return Ok(result);
        }

        [HttpGet("unread/{userId}")]
        public async Task<IActionResult> GetUnreadCount(int userId)
        {
            var result = await _mediator.Send(
                new GetUnreadCountQuery(userId));

            return Ok(result);
        }

        [HttpPut("read/{notificationId}")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            await _mediator.Send(
                new MarkNotificationAsReadCommand(notificationId));

            return NoContent();
        }

        [HttpPut("read-all/{userId}")]
        public async Task<IActionResult> MarkAllAsRead(int userId)
        {
            await _mediator.Send(
                new MarkAllNotificationsAsReadCommand(userId));

            return NoContent();
        }
    }
}
