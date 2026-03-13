using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Queries.GetUnreadCount
{
    public record GetUnreadCountQuery(int UserId)
         : IRequest<int>;
}
