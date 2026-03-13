using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Interfaces
{
    public interface IRealtimeNotificationService
    {
        Task SendAsync(int userId, string title, string message);
    }
}
